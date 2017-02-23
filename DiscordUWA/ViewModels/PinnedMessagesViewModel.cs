using Discord;
using Discord.WebSocket;
using DiscordUWA.Common;
using DiscordUWA.Interfaces;
using DiscordUWA.Models;
using DiscordUWA.Services;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;

namespace DiscordUWA.ViewModels {
    public class PinnedMessagesViewModel : ViewModelBase {
        private ulong channelId = 0L;

        private RangeObservableCollection<ChatTextListModel> chatLogList = new RangeObservableCollection<ChatTextListModel>();
        public RangeObservableCollection<ChatTextListModel> ChatLogList {
            get { return this.chatLogList; }
            set { SetProperty(ref chatLogList, value); }
        }

        public override async Task OnNavigatedToAsync(object parameter) {
            var id = parameter as ulong?;
            if (id.HasValue) {
                channelId = id.Value;
                await PopulateMessageLog(id.Value);
            }
            // Add back button to titlebar
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }
        public override Task OnNavigatedFromAsync(object parameter) {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            return Task.CompletedTask;
        }

        private async Task PopulateMessageLog(ulong channelId) {
            ChatLogList.Clear();
            var channel = LocatorService.DiscordSocketClient.GetChannel(channelId) as SocketTextChannel;
            var messageLog = await channel.GetPinnedMessagesAsync().ConfigureAwait(false);

            foreach (var message in messageLog) {
                AddChatMessageToChatLog(message);
            }
        }

        private void AddChatMessageToChatLog(IMessage message) {
            Color roleColor = new Color(0xff, 0xff, 0xff);
            var channel = LocatorService.DiscordSocketClient.GetChannel(channelId) as SocketGuildChannel;
            var guildUser = channel.GetUser(message.Author.Id);
            // todo: figure out how to pick 'highest' role and take that color
            foreach (var roleid in guildUser.RoleIds) {
                var role = channel.Guild.GetRole(roleid);
                if (!role.IsEveryone) {
                    roleColor = role.Color;
                }
            }

            // Serialize UI update to the main UI thread
            DispatcherHelper.CheckBeginInvokeOnUI(() => {
                ChatLogList.Add(new ChatTextListModel {
                    Id = message.Id,
                    Username = message.Author.Nickname ? message.Author.Nickname : message.Author.Username,
                    UserRoleColor = roleColor.ToWinColor(),
                    ChatText = message.GetReplacedMessageText(),
                    TimeSent = message.Timestamp.ToLocalTime().ToString("g"),
                    TimeEdited = message.EditedTimestamp?.ToLocalTime().ToString("g"),
                    AvatarUrl = message.Author.AvatarUrl,
                    Embeds = message.Embeds,
                    Attachments = message.Attachments
                });
            });
        }

    }
}
