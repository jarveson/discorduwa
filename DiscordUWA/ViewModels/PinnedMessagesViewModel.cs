using DiscordUWA.Common;
using DiscordUWA.Interfaces;
using DiscordUWA.Services;
using System;
using System.Windows.Input;
using Windows.UI.Core;

namespace DiscordUWA.ViewModels {
    public class PinnedMesssagesViewModel : BindableBase, INavigable {
        private RangeObservableCollection<ChatTextListModel> chatLogList = new RangeObservableCollection<ChatTextListModel>();
        public RangeObservableCollection<ChatTextListModel> ChatLogList {
            get { return this.chatLogList; }
            set { SetProperty(ref chatLogList, value); }
        }

        public async void OnNavigatingTo(object parameter) {
            var id = parameter as ulong?;
            if (id.HasValue) {
                await PopulateMessageLog(id);
            }
            // Add back button to titlebar
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }
        public void OnNavigatingFrom(object parameter) {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
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
            var server = LocatorService.DiscordSocketClient.GetGuild(selectedGuildId);
            var guildUser = server.GetUser(message.Author.Id);
            // todo: figure out how to pick 'highest' role and take that color
            foreach (var roleid in guildUser.RoleIds) {
                var role = server.GetRole(roleid);
                if (!role.IsEveryone) {
                    roleColor = role.Color;
                }
            }

            string imageUrl = null;
            // 'pictures' can also just be attachements -_-
            foreach (var attachment in message.Attachments) {
                // shameless hack around dealing with this
                if (attachment.Height != null && attachment.Width != null) {
                    imageUrl = attachment.ProxyUrl;
                }
            }
            foreach (var embed in message.Embeds) {
                imageUrl = embed.Thumbnail?.Url;
            }
            // Serialize UI update to the main UI thread
            DispatcherHelper.CheckBeginInvokeOnUI(() => {
                ChatLogList.Add(new ChatTextListModel(message.Author.Username, roleColor.ToWinColor(), message.Content,
                    message.Timestamp.ToString("h:mm tt"), imageUrl, message.Author.AvatarUrl));
            });
        }

    }
}
