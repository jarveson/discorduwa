using DiscordUWA.Common;
using DiscordUWA.Models;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DiscordUWA.UserControls {
    public sealed partial class GuildPane : UserControl {

        public static DependencyProperty ShowUserListProperty { get; } = DependencyProperty.Register(
            nameof(ShowUserList),
            typeof(bool),
            typeof(GuildPane),
            new PropertyMetadata("")
            );

        public bool ShowUserList {
            get { return (bool)GetValue(ShowUserListProperty); }
            set { SetValue(ShowUserListProperty, value); }
        }

        public static DependencyProperty ChannelNameProperty { get; } = DependencyProperty.Register(
            nameof(ChannelName),
            typeof(string),
            typeof(GuildPane),
            new PropertyMetadata("")
            );

        public string ChannelName {
            get { return (string)GetValue(ChannelNameProperty); }
            set { SetValue(ChannelNameProperty, value); }
        }

        public static DependencyProperty MessageTextProperty { get; } = DependencyProperty.Register(
            nameof(MessageText),
            typeof(string),
            typeof(GuildPane),
            new PropertyMetadata("")
            );

        public string MessageText {
            get { return (string)GetValue(MessageTextProperty); }
            set { SetValue(MessageTextProperty, value); }
        }

        public static DependencyProperty SendMessageCommandProperty { get; } = DependencyProperty.Register(
            nameof(SendMessageCommand),
            typeof(ICommand),
            typeof(GuildPane),
            new PropertyMetadata(null)
            );

        public ICommand SendMessageCommand {
            get { return (ICommand)GetValue(SendMessageCommandProperty); }
            set { SetValue(SendMessageCommandProperty, value); }
        }

        public static DependencyProperty UserClickCommandProperty { get; } = DependencyProperty.Register(
            nameof(UserClick),
            typeof(ICommand),
            typeof(GuildPane),
            new PropertyMetadata(null)
            );

        public ICommand UserClick {
            get { return (ICommand)GetValue(UserClickCommandProperty); }
            set { SetValue(UserClickCommandProperty, value); }
        }

        public static DependencyProperty ServerListToggleCommandProperty { get; } = DependencyProperty.Register(
            nameof(ServerListToggle),
            typeof(ICommand),
            typeof(GuildPane),
            new PropertyMetadata(null)
            );

        public ICommand ServerListToggle {
            get { return (ICommand)GetValue(ServerListToggleCommandProperty); }
            set { SetValue(ServerListToggleCommandProperty, value); }
        }

        public static DependencyProperty ChatLogListProperty { get; } = DependencyProperty.Register(
            nameof(ChatLogList),
            typeof(IEnumerable<ChatTextListModel>),
            typeof(GuildPane),
            new PropertyMetadata(null)
            );

        public IEnumerable<ChatTextListModel> ChatLogList {
            get { return (IEnumerable<ChatTextListModel>)GetValue(ChatLogListProperty); }
            set { SetValue(ChatLogListProperty, value); }
        }

        public static DependencyProperty ChannelTopicProperty { get; } = DependencyProperty.Register(
            nameof(ChannelTopic),
            typeof(string),
            typeof(GuildPane),
            new PropertyMetadata("")
            );

        public string ChannelTopic {
            get { return (string)GetValue(ChannelTopicProperty); }
            set { SetValue(ChannelTopicProperty, value); }
        }

        public static DependencyProperty FullUserListProperty { get; } = DependencyProperty.Register(
            nameof(FullUserList),
            typeof(IEnumerable<UserListSectionModel>),
            typeof(UserList),
            new PropertyMetadata(null)
            );

        public IEnumerable<UserListSectionModel> FullUserList {
            get { return (IEnumerable<UserListSectionModel>)GetValue(FullUserListProperty); }
            set { SetValue(FullUserListProperty, value); }
        }

        public static DependencyProperty ToggleUserListCommandProperty { get; } = DependencyProperty.Register(
            nameof(ToggleUserListCommand),
            typeof(ICommand),
            typeof(UserList),
            new PropertyMetadata(null)
            );

        public ICommand ToggleUserListCommand {
            get { return (ICommand)GetValue(ToggleUserListCommandProperty); }
            set { SetValue(ToggleUserListCommandProperty, value); }
        }

        public GuildPane() {
            this.InitializeComponent();
        }

    }
}