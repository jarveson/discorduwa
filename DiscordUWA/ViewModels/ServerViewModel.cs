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
using DiscordUWA.Controls;

namespace DiscordUWA.ViewModels {
    public class ServerViewModel : ViewModelBase {
        private ulong selectedGuildId = 0L;
        private ulong channelId = 0L;
        private ulong lastAuthorId = 0L;

        private Throttler typingThrottler = new Throttler(TimeSpan.FromSeconds(5));

        public ICommand LoadJoinedServersList { protected set; get; }
        public ICommand LoadCurrentUserAvatar { protected set; get; }
        public ICommand SendMessageToCurrentChannel { protected set; get; }
        public ICommand ToggleUserListCommand { protected set; get; }
        public ICommand PinnedMessagesCommand { protected set; get; }

        public ICommand UserClick { protected set; get; }

        public ICommand ServerListToggle {protected set; get;}

        public ICommand LinkClickCommand { protected set; get;}

        public ICommand OpenDMCommand { protected set; get; }

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
                    if (channelId == 0) return;
                    typingThrottler.ResetAndTick();
                }
            }
        }

        private bool showUserList;
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
            typingThrottler.Action += {
                DispatcherHelper.CheckBeginInvokeOnUI(async () => {
                    await (LocatorService.DiscordSocketClient.GetChannel(channelId) as SocketTextChannel).TriggerTypingAsync();
                });
            };

            LocatorService.DiscordSocketClient.MessageReceived += DiscordClient_MessageReceived;
            LocatorService.DiscordSocketClient.MessageUpdated += DiscordClient_MessageUpdated;
            LocatorService.DiscordSocketClient.MessageDeleted += DiscordClient_MessageDeleted;
            LocatorService.DiscordSocketClient.UserUpdated += DiscordClient_UserUpdated;

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
                if (channelId == 0) return;
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

            this.LinkClickCommand = new DelegateCommand<LinkClickedEventArgs>(async (args) => {
                // todo: rest of these
                if (args.Link.StartsWith("http"))
                    await Windows.System.Launcher.LaunchUriAsync(new Uri(args.Link));
            });

            this.OpenDMCommand = new DelegateCommand(() => {
                ChannelList.Clear();
                selectedGuildId = 0L;

                foreach (var channel in LocatorService.DiscordSocketClient.DMChannels) {
                    string temp = channel.Name;
                    ChannelList.Add(new ChannelListModel {
                        ChannelId = channel.Id,
                        ChannelName = temp,
                    });
                }
            });
        }

        private async Task SelectChannel(ulong selectedChannelId) {
            channelId = selectedChannelId;
            await PopulateChatLog();

            if (selectedGuildId == 0L) {
                var channel = LocatorService.DiscordSocketClient.GetChannel(channelId) as SocketDMChannel;
                CurrentChannelName = channel.Name;
                ChannelTopic = channel.Topic;
            }
            else {
                var channel = LocatorService.DiscordSocketClient.GetChannel(channelId) as SocketTextChannel;
                CurrentChannelName = channel.Name;
                ChannelTopic = channel.Topic;
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

            // Serialize UI update to the main UI thread
            bool sameAuthor = message.Author.Id == lastAuthorId;
            lastAuthorId = message.Author.Id;

            DispatcherHelper.CheckBeginInvokeOnUI(() => {
                if (sameAuthor)
                    ChatLogList.Add(new ChatTextListModel {
                        Id = message.Id,
                        Username = "",
                        UserRoleColor = roleColor.ToWinColor(),
                        ChatText = message.GetReplacedMessageText(),
                        TimeSent = message.Timestamp.ToLocalTime().ToString("g"),
                        TimeEdited = message.EditedTimestamp?.ToLocalTime().ToString("g"),
                        AvatarUrl = "",
                        Embeds = message.Embeds,
                        Attachments = message.Attachments
                    });
                else
                    ChatLogList.Add(new ChatTextListModel {
                        Id = message.Id,
                        Username = message.Author.Username,
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

        private Task DiscordClient_MessageReceived(SocketMessage message) {
            if (message.Channel.Id != channelId) {
                return Task.CompletedTask;
            }

            AddChatMessageToChatLog(message);
            return Task.CompletedTask;
        }

        private Task DiscordClient_MessageUpdated(Message before, Message after) {
            if (before.channelId != selectedChannelId)
                return Task.CompletedTask;

            var found = ChatLogList.FirstOrDefault(x=>x.Id == before.Id);
            // didnt find
            if (found.Id == 0)
                return Task.CompletedTask;

            DispatcherHelper.CheckBeginInvokeOnUI(() => {
                found.ChatText = after.GetReplacedMessageText();
                found.TimeEdited = after.EditedTimestamp?.ToLocalTime().ToString("g"),
                found.Embeds = after.Embeds;
                found.Attachments = after.Attachments;

                OnPropertyChanged("ChatLogList");
            });
        }

        private Task DiscordClient_MessageDeleted(Message msg) {
            if (before.channelId != selectedChannelId)
                return Task.CompletedTask;
            var found = ChatLogList.FirstOrDefault(x=>x.Id == msg.Id);
            if (found.Id == 0)
                return Task.CompletedTask;
            DispatcherHelper.CheckBeginInvokeOnUI(() => {
                ChatLogList.Remove(found);
            });
        }

        private Task DiscordClient_UserUpdated(User before, User after) {
            // todo: fix userlist to be a control or something
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
                    // this seems to work for the time being though
                    IRole foundRole = null;
                    foreach (var roleid in user.RoleIds) {
                        var role = server.GetRole(roleid);
                        roleColor = role.Color;
                        if (role.IsHoisted) {
                            if (user.Status == UserStatus.Offline || user.Status == UserStatus.Unknown)
                                break;

                            foundRole = role;
                        }
                    }
                    if (foundRole == null) {
                        if (user.Status == UserStatus.Offline || user.Status == UserStatus.Unknown)
                            tmpOffline.Add(new UserListSectionModel {
                                AvatarUrl = user.AvatarUrl,
                                CurrentlyPlaying = user.Game.HasValue ? user.Game.Value.Name : "",
                                StatusColor = user.Status.ToWinColor(),
                                Username = user.Author.Nickname ? user.Author.Nickname : user.Author.Username,
                                UserRoleColor = roleColor.ToWinColor(),
                                Id = user.Id,
                                IsBot = user.IsBot,
                                Nickname = user.Nickname
                            });
                        else
                            tmpOnline.Add(new UserListSectionModel {
                                AvatarUrl = user.AvatarUrl,
                                CurrentlyPlaying = user.Game.HasValue ? user.Game.Value.Name : "",
                                StatusColor = user.Status.ToWinColor(),
                                Username = user.Author.Nickname ? user.Author.Nickname : user.Author.Username,
                                UserRoleColor = roleColor.ToWinColor(),
                                Id = user.Id,
                                IsBot = user.IsBot,
                                Nickname = user.Nickname
                            });
                    }
                    else {
                        if (!tmpUserSort.ContainsKey(foundRole)) {
                            tmpUserSort[foundRole] = new List<UserListSectionModel> {
                                new UserListSectionModel {
                                    AvatarUrl = user.AvatarUrl,
                                    CurrentlyPlaying = user.Game.HasValue ? user.Game.Value.Name : "",
                                    StatusColor = user.Status.ToWinColor(),
                                    Username = user.Username,
                                    UserRoleColor = roleColor.ToWinColor(),
                                    Id = user.Id,
                                    IsBot = user.IsBot,
                                    Nickname = user.Nickname
                                }
                            };
                        }
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

                        fullUserList.AddRange(tmpUserSort[key]);
                    }
                    if (tmpOnline.Count > 0) {
                        fullUserList.Add(new UserListSectionModel {
                            RoleSectionName = "Online",
                            NumUsers = (uint)tmpOnline.Count
                        });
                        fullUserList.AddRange(tmpOnline.OrderBy(x => x.Username));
                    }
                    if (tmpOffline.Count > 0) {
                        fullUserList.Add(new UserListSectionModel {
                            RoleSectionName = "Offline",
                            NumUsers = (uint)tmpOffline.Count
                        });
                        fullUserList.AddRange(tmpOffline.OrderBy(x => x.Username));
                    }
                });
            });
        }

        private async Task PopulateChatLog() {
            ChatLogList.Clear();
            lastAuthorId = 0L;
            IAsyncEnumerable<IReadOnlyCollection<IMessage>> messageLog = null;
            // todo: use message.type or something
            if (selectedGuildId == 0L) {
                var channel = LocatorService.DiscordSocketClient.GetChannel(channelId) as SocketDMChannel;
                messageLog = await channel.GetMessagesAsync(40).Flatten().ConfigureAwait(false);
            }
            else {
                var channel = LocatorService.DiscordSocketClient.GetChannel(channelId) as SocketTextChannel;
                messageLog = await channel.GetMessagesAsync(40).Flatten().ConfigureAwait(false);
            }

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
