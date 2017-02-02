using Discord;
using DiscordUWA.Common;
using DiscordUWA.Interfaces;
using DiscordUWA.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using DiscordUWA.Services;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight.Threading;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace DiscordUWA.ViewModels {
    public class ServerViewModel : BindableBase {
        private ulong serverId = 0L;
        private ulong channelId = 0L;

        public ICommand LoadJoinedServersList { protected set; get; }
        public ICommand LoadCurrentUserAvatar { protected set; get; }

        public ICommand SelectServer { protected set; get; }
        public ICommand SelectChannel { protected set; get; }

        public ICommand SendMessageToCurrentChannel { protected set; get; }

        private ObservableCollection<ServerListModel> serverListModelList = new ObservableCollection<ServerListModel>();
        public ObservableCollection<ServerListModel> ServerListModelList {
            get { return this.serverListModelList; }
            set { SetProperty(ref serverListModelList, value); }
        }

        private ObservableCollection<ChannelListModel> channelList = new ObservableCollection<ChannelListModel>();
        public ObservableCollection<ChannelListModel> ChannelList {
            get { return this.channelList; }
            set { SetProperty(ref channelList, value); }
        }

        private ObservableCollection<UserListModel> offlineUserList = new ObservableCollection<UserListModel>();
        public ObservableCollection<UserListModel> OfflineUserList {
            get { return this.offlineUserList; }
            set { SetProperty(ref offlineUserList, value); }
        }

        private ObservableCollection<UserListModel> onlineUserList = new ObservableCollection<UserListModel>();
        public ObservableCollection<UserListModel> OnlineUserList {
            get { return this.onlineUserList; }
            set { SetProperty(ref onlineUserList, value); }
        }

        // observables dont need setproperty on outer, just on inner properties?
        public ObservableCollection<ChatTextListModel> ChatLogList { get; set; }

        private string currentServerName;
        public string CurrentServerName {
            get { return this.currentServerName; }
            set { SetProperty(ref currentServerName, value); }
        }

        private string currentChannelName;
        public string CurrentChannelName {
            get { return this.currentChannelName; }
            set { SetProperty(ref currentChannelName, value); }
        }

        private Uri currentUserAvatarUrl;
        public Uri CurrentUserAvatarUrl {
            get { return this.currentUserAvatarUrl; }
            set { SetProperty(ref currentUserAvatarUrl, value); }
        }

        private string currentChatMessage;
        public string CurrentChatMessage {
            get { return this.currentChatMessage; }
            set { SetProperty(ref currentChatMessage, value); }
        }

        public ServerViewModel() {
            this.ChatLogList = new ObservableCollection<ChatTextListModel>();

            LocatorService.DiscordSocketClient.MessageReceived += DiscordClient_MessageReceived;

            this.LoadJoinedServersList = new DelegateCommand(() => {
                ServerListModelList.Clear();
                var servers = LocatorService.DiscordSocketClient.Guilds;

                foreach (var server in servers) {
                    //serverId = server.Id;
                    string serverIconUrl = server.IconUrl == null ? "x" : server.IconUrl;
                    ServerListModelList.Add(new ServerListModel(server.Id, server.Name, serverIconUrl));
                }
            });

            this.LoadCurrentUserAvatar = new DelegateCommand(() => {
                CurrentUserAvatarUrl = new Uri(LocatorService.DiscordSocketClient.CurrentUser.AvatarUrl);
            });

            this.SelectServer = new DelegateCommand<ulong?>((selectedServerId) => {
                if (selectedServerId == null) return;
                serverId = selectedServerId.Value;
                var server = LocatorService.DiscordSocketClient.GetGuild(serverId);
                CurrentServerName = server.Name;
                PopulateChannelList();
            });

            this.SelectChannel = new DelegateCommand<ulong?>((selectedChannelId) => {
                if (selectedChannelId == null) return;
                channelId = selectedChannelId.Value;

                var channel = LocatorService.DiscordSocketClient.GetChannel(channelId) as SocketTextChannel;
                CurrentChannelName = channel.Name;

                PopulateChatLog();
                PopulateUserList();
            });

            this.SendMessageToCurrentChannel = new DelegateCommand(async () => {
                var message = currentChatMessage;
                var channel = LocatorService.DiscordSocketClient.GetChannel(channelId) as SocketTextChannel;
                await channel.SendMessageAsync(message);

                CurrentChatMessage = String.Empty;
            });
        }

        private void AddChatMessageToChatLog(IMessage message) {
            Color roleColor = new Color(0xff, 0xff, 0xff);
            var server = LocatorService.DiscordSocketClient.GetGuild(serverId);
            var guildUser = server.GetUser(message.Author.Id);
            // todo: figure out how to pick 'highest' role and take that color
            foreach (var roleid in guildUser.RoleIds) {
                var role = server.GetRole(roleid);
                if (!role.IsEveryone) {
                    roleColor = role.Color;
                }
            }

            string imageUrl = "x";
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
            DispatcherHelper.CheckBeginInvokeOnUI( () => {
                ChatLogList.Add(new ChatTextListModel(message.Author.Username, roleColor.ToString(), message.Content,
                    message.Timestamp.ToString("h:mm tt"), imageUrl));
            });
        }

        private Task DiscordClient_MessageReceived(SocketMessage message) {
            if (message.Channel.Id != channelId) {
                return Task.CompletedTask;
            }

            AddChatMessageToChatLog(message);
            return Task.CompletedTask;
        }

        private void PopulateChannelList() {
            ChannelList.Clear();
            if (serverId == 0L) return;
            var server = LocatorService.DiscordSocketClient.GetGuild(serverId);

            foreach (var channel in server.TextChannels) {
                string temp = $"# {channel.Name}";
                ChannelList.Add(new ChannelListModel(channel.Id, temp));
            }
        }

        private void PopulateUserList() {
            onlineUserList.Clear();
            offlineUserList.Clear();

            var server = LocatorService.DiscordSocketClient.GetGuild(serverId);

            foreach (var user in channel.Users) {
                Color roleColor = Color.Default;
                // todo: figure out how to pick 'highest' role and take that color
                foreach (var roleid in user.RoleIds) {
                    var role = server.GetRole(roleid);
                    if (!role.IsEveryone) {
                        roleColor = role.Color;
                    }
                }

                if (user.Status == Discord.UserStatus.Offline || user.Status == UserStatus.Unknown) {
                    OfflineUserList.Add(new UserListModel(user.AvatarUrl, "", false, user.Username, roleColor.RawValue == 0 ? "#99ffffff" : roleColor.ToString()));
                }

                else {
                    OnlineUserList.Add(new UserListModel(user.AvatarUrl, user.Game.HasValue ? user.Game.Value.Name : "", (user.Status == Discord.UserStatus.Idle),
                        user.Username, roleColor.RawValue == 0 ? "#99ffffff" : roleColor.ToString()));
                }
            }
        }

        private void PopulateChatLog() {
            ChatLogList.Clear();
            var channel = LocatorService.DiscordSocketClient.GetChannel(channelId) as SocketTextChannel;
            var messageLog = await channel.GetMessagesAsync(40).Flatten();
            foreach (var message in messageLog.Reverse()) {
                AddChatMessageToChatLog(message);
            }
        }
    }
}
