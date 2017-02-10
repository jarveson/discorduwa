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
using System.Collections.Generic;

namespace DiscordUWA.ViewModels {
    public class ServerViewModel : ViewModelBase {
        private ulong selectedGuildId = 0L;
        private ulong channelId = 0L;
        private ulong lastAuthorId = 0L;

        public ICommand LoadJoinedServersList { protected set; get; }
        public ICommand LoadCurrentUserAvatar { protected set; get; }
        public ICommand SendMessageToCurrentChannel { protected set; get; }
        public ICommand ToggleUserListCommand { protected set; get; }
        public ICommand PinnedMessagesCommand {protected set; get;}

        public ICommand UserClick { protected set; get; }

        public ICommand ServerListToggle {protected set; get;}

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

        private RangeObservableCollection<UserListSectionModel> fullUserList = new RangeObservableCollection<UserListSectionModel>();
        public RangeObservableCollection<UserListSectionModel> FullUserList {
            get { return this.fullUserList; }
            set { SetProperty(ref fullUserList, value); }
        }

        private RangeObservableCollection<ChatTextListModel> chatLogList = new RangeObservableCollection<ChatTextListModel>();
        public RangeObservableCollection<ChatTextListModel> ChatLogList {
            get { return this.chatLogList; }
            set { SetProperty(ref chatLogList, value); }
        }

        private ChannelListModel selectedChannel; 
        public ChannelListModel SelectedChannel {
            get { return this.selectedChannel; }
            set {
                if (SetProperty(ref selectedChannel, value) && value != null) {
                    DispatcherHelper.CheckBeginInvokeOnUI(async () => {
                        await SelectChannel(value.ChannelId);
                    });
                }
            }
        }

        private ServerListModel selectedGuild; 
        public ServerListModel SelectedGuild {
            get { return this.selectedGuild; }
            set {
                if (SetProperty(ref selectedGuild, value) && value != null) {
                    DispatcherHelper.CheckBeginInvokeOnUI(async () => {
                        await SelectServer(value.ServerId);
                    });
                }
            }
        }

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

        private string currentUserAvatarUrl;
        public string CurrentUserAvatarUrl {
            get { return this.currentUserAvatarUrl; }
            set { SetProperty(ref currentUserAvatarUrl, value); }
        }

        private string currentUserName;
        public string CurrentUserName {
            get { return this.currentUserName;}
            set { SetProperty(ref currentUserName, value); }
        }

        private string currentUserDescrim;
        public string CurrentUserDescrim {
            get { return this.currentUserDescrim;}
            set { SetProperty(ref currentUserDescrim, value); }
        }


        private string currentChatMessage;
        public string CurrentChatMessage {
            get { return this.currentChatMessage; }
            set { 
                if (SetProperty(ref currentChatMessage, value) && !String.IsNullOrEmpty(value)) {
                    DispatcherHelper.CheckBeginInvokeOnUI(async () => {
                        await (LocatorService.DiscordSocketClient.GetChannel(channelId) as SocketTextChannel).TriggerTypingAsync();
                    });
                }
            }
        }

        private bool showUserList = true;
        public bool ShowUserList {
            get { return this.showUserList; }
            set { SetProperty(ref showUserList, value); }
        }

        private bool showServerPane = true;
        public bool ShowServerPane {
            get { return this.showServerPane; }
            set { SetProperty(ref showServerPane, value); }
        }

        private string channelTopic = "";
        public string ChannelTopic {
            get { return this.channelTopic; }
            set { SetProperty(ref channelTopic, value); }
        }

        public override Task OnNavigatedToAsync(object parameter) {
            LoadJoinedServersList.Execute(null);
            LoadCurrentUserAvatar.Execute(null);
            var currentUser = LocatorService.DiscordSocketClient.CurrentUser;
            CurrentUserName = currentUser.Username;
            CurrentUserDescrim = $"#{currentUser.Discriminator}";
            return Task.CompletedTask;
        }

        public ServerViewModel() {

            LocatorService.DiscordSocketClient.MessageReceived += DiscordClient_MessageReceived;

            this.LoadJoinedServersList = new DelegateCommand(() => {
                ServerListModelList.Clear();
                var servers = LocatorService.DiscordSocketClient.Guilds;

                foreach (var server in servers) {
                    ServerListModelList.Add(new ServerListModel {
                        ServerId = server.Id,
                        ServerName = server.Name,
                        ServerImageUri = server.IconUrl,
                    });
                }
            });

            this.LoadCurrentUserAvatar = new DelegateCommand(() => {
                CurrentUserAvatarUrl = LocatorService.DiscordSocketClient.CurrentUser.AvatarUrl;
            });

            this.SendMessageToCurrentChannel = new DelegateCommand(async () => {
                var message = currentChatMessage;
                if (String.IsNullOrEmpty(message)) return;
                var channel = LocatorService.DiscordSocketClient.GetChannel(channelId) as SocketTextChannel;
                await channel.SendMessageAsync(message);

                CurrentChatMessage = String.Empty;
            });

            this.ToggleUserListCommand = new DelegateCommand(() => {
                ShowUserList = !ShowUserList;
            });

            this.UserClick = new DelegateCommand<ulong>((userId) => {
                // 0 for now should mean its the role header
                if (userId != 0)
                    LocatorService.NavigationService.NavigateTo("userProfile", userId);
            });

            this.ServerListToggle = new DelegateCommand(() => {
                ShowServerPane = !ShowServerPane;
            });

            this.PinnedMessagesCommand = new DelegateCommand(() => {
                if (channelId != 0)
                    LocatorService.NavigationService.NavigateTo("pinnedMessages", channelId);
            });
        }

        private async Task SelectChannel(ulong selectedChannelId) {
            channelId = selectedChannelId;
            await PopulateChatLog();

            var channel = LocatorService.DiscordSocketClient.GetChannel(channelId) as SocketTextChannel;
            CurrentChannelName = channel.Name;
            ChannelTopic = channel.Topic;
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

            // 'pictures' can also just be attachements -_-
            /*foreach (var attachment in message.Attachments) {
                // shameless hack around dealing with this
                if (attachment.Height != null && attachment.Width != null) {
                    imageUrl = attachment.ProxyUrl;
                }
            }*/
            // Serialize UI update to the main UI thread
            bool sameAuthor = message.Author.Id == lastAuthorId;
            lastAuthorId = message.Author.Id;
            DispatcherHelper.CheckBeginInvokeOnUI(() => {
                if (sameAuthor)
                    ChatLogList.Add(new ChatTextListModel {
                        Username = "",
                        UserRoleColor = roleColor.ToWinColor(),
                        ChatText = message.Content,
                        TimeSent = message.Timestamp.ToString("h:mm tt"),
                        AvatarUrl = "",
                        Embeds = message.Embeds
                    });
                else
                    ChatLogList.Add(new ChatTextListModel {
                        Username = message.Author.Username,
                        UserRoleColor = roleColor.ToWinColor(),
                        ChatText = message.Content,
                        TimeSent = message.Timestamp.ToString("h:mm tt"),
                        AvatarUrl = message.Author.AvatarUrl,
                        Embeds = message.Embeds
                    });
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
            if (selectedGuildId == 0L) return;
            var server = LocatorService.DiscordSocketClient.GetGuild(selectedGuildId);

            foreach (var channel in server.TextChannels.OrderBy(x => x.Position)) {
                string temp = $"# {channel.Name}";
                ChannelList.Add(new ChannelListModel {
                    ChannelId = channel.Id,
                    ChannelName = temp,
                });
            }
        }

        private async Task PopulateUserList() {
            fullUserList.Clear();
            var server = LocatorService.DiscordSocketClient.GetGuild(selectedGuildId);

            await Task.Run(() => {
                List<UserListSectionModel> tmpOffline = new List<UserListSectionModel>();
                List<UserListSectionModel> tmpOnline = new List<UserListSectionModel>();

                SortedDictionary<IRole, List<UserListSectionModel>> tmpUserSort = new SortedDictionary<IRole, List<UserListSectionModel>>();

                foreach (var user in server.Users) {
                    Color roleColor = Color.Default;

                    // todo: figure out how to pick 'highest' role and take that color
                    bool foundRole = false;
                    foreach (var roleid in user.RoleIds) {
                        var role = server.GetRole(roleid);
                        roleColor = role.Color;
                        if (role.IsHoisted) {
                            if (user.Status == UserStatus.Offline || user.Status == UserStatus.Unknown)
                                break;

                            foundRole = true;
                            if (!tmpUserSort.ContainsKey(role))
                                tmpUserSort[role] = new List<UserListSectionModel>();
                                tmpUserSort[role].Add(new UserListSectionModel {
                                AvatarUrl = user.AvatarUrl,
                                CurrentlyPlaying = user.Game.HasValue ? user.Game.Value.Name : "",
                                StatusColor = user.Status.ToWinColor(),
                                Username = user.Username,
                                UserRoleColor = roleColor.ToWinColor(),
                                Id = user.Id,
                                IsBot = user.IsBot,
                            }); 
                            break;
                        }
                    }
                    if (!foundRole) {
                        if (user.Status == UserStatus.Offline || user.Status == UserStatus.Unknown)
                            tmpOffline.Add(new UserListSectionModel {
                                AvatarUrl = user.AvatarUrl,
                                CurrentlyPlaying = user.Game.HasValue ? user.Game.Value.Name : "",
                                StatusColor = user.Status.ToWinColor(),
                                Username = user.Username,
                                UserRoleColor = roleColor.ToWinColor(),
                                Id = user.Id,
                                IsBot = user.IsBot,
                            });
                        else
                            tmpOnline.Add(new UserListSectionModel {
                                AvatarUrl = user.AvatarUrl,
                                CurrentlyPlaying = user.Game.HasValue ? user.Game.Value.Name : "",
                                StatusColor = user.Status.ToWinColor(),
                                Username = user.Username,
                                UserRoleColor = roleColor.ToWinColor(),
                                Id = user.Id,
                                IsBot = user.IsBot,
                            });
                    }
                }
                // todo: can we use DeferRefresh here instead of this other range thing?
                // or is that advancedcollectionview specific?
                DispatcherHelper.CheckBeginInvokeOnUI(() => {
                    foreach(var key in tmpUserSort.Keys) {
                        fullUserList.Add(new UserListSectionModel {
                            RoleSectionName = key.Name,
                            NumUsers = (uint)tmpUserSort[key].Count,
                        });

                        foreach (var value in tmpUserSort[key])
                            fullUserList.Add(value);
                    }
                    if (tmpOnline.Count > 0) {
                        fullUserList.Add(new UserListSectionModel {
                            RoleSectionName = "Online",
                            NumUsers = (uint)tmpOnline.Count
                        });
                        foreach (var user in tmpOnline.OrderBy(x => x.Username))
                            fullUserList.Add(user);
                    }
                    if (tmpOffline.Count > 0) {
                        fullUserList.Add(new UserListSectionModel {
                            RoleSectionName = "Offline",
                            NumUsers = (uint)tmpOffline.Count
                        });
                        foreach (var user in tmpOffline.OrderBy(x => x.Username))
                            fullUserList.Add(user);
                    }
                });
            });
        }

        private async Task PopulateChatLog() {
            ChatLogList.Clear();
            lastAuthorId = 0L;
            var channel = LocatorService.DiscordSocketClient.GetChannel(channelId) as SocketTextChannel;
            var messageLog = await channel.GetMessagesAsync(40).Flatten().ConfigureAwait(false);

            foreach (var message in messageLog.Reverse()) {
                AddChatMessageToChatLog(message);
            }
        }

        private async Task SelectServer(ulong selectedServerId) {
            selectedGuildId = selectedServerId;
            var server = LocatorService.DiscordSocketClient.GetGuild(selectedGuildId);
            await PopulateUserList();
            CurrentServerName = server.Name;
            PopulateChannelList();
            SelectedChannel = channelList.SingleOrDefault(x => x.ChannelId == server.DefaultChannel.Id);
        }
    }
}
