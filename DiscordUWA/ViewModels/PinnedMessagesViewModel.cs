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
    public class PinnedMessagesNavData {
        public ulong ChannelId { get; set; } = 0;
        public ulong GuildId { get; set; } = 0;
    }

    public class PinnedMessagesViewModel : ViewModelBase {
        private ulong channelId = 0L;
        private ulong guildId = 0L;

        private RangeObservableCollection<ChatTextListModel> chatLogList = new RangeObservableCollection<ChatTextListModel>();
        public RangeObservableCollection<ChatTextListModel> ChatLogList {
            get { return this.chatLogList; }
            set { SetProperty(ref chatLogList, value); }
        }

        public override async Task OnNavigatedToAsync(object parameter) {
            if (parameter is PinnedMessagesNavData data) {
                channelId = data.ChannelId;
                guildId = data.GuildId;
                await PopulateMessageLog(channelId);
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
                // todo: nickname?
                ChatLogList.Add(new ChatTextListModel {
                    Id = message.Id,
                    Username = message.Author.Username,
                    UserRoleColor = roleColor.ToWinColor(),
                    ChatText = GetReplacedMessageText(message),
                    TimeSent = message.Timestamp.ToLocalTime().ToString("g"),
                    TimeEdited = message.EditedTimestamp?.ToLocalTime().ToString("g"),
                    AvatarUrl = message.Author.AvatarUrl,
                    Embeds = message.Embeds,
                    Attachments = message.Attachments
                });
            });
        }

        private string GetReplacedMessageText(IMessage message) {
            string chatText = message.Content;
            // whats going to happen here is we change the discord tag to also include the text
            // todo: theres probably a better / faster way to do this?
            var guild = LocatorService.DiscordSocketClient.GetGuild(guildId);
            foreach (var userId in message.MentionedUserIds) {
                // nickname
                var user = guild.GetUser(userId);
                chatText = chatText.Replace($"<@!{user.Id}>", $"<@!{user.Id}:{user.Nickname}>");
                // regular name
                string name = string.IsNullOrEmpty(user.Nickname) ? user.Username : user.Nickname;
                chatText = chatText.Replace($"<@{user.Id}>", $"<@{user.Id}:{name}>");
            }
            foreach (var roleId in message.MentionedRoleIds) {
                var role = guild.GetRole(roleId);
                chatText = chatText.Replace($"<@&{role.Id}>", $"<@&{role.Id}:{role.Name}>");
            }
            foreach (var channelId in message.MentionedChannelIds) {
                var channel = guild.GetChannel(channelId);
                chatText = chatText.Replace($"<@#{channel.Id}>", $"<@#{channel.Id}:{channel.Name}>");
            }

            return chatText;
        }
    }
}
