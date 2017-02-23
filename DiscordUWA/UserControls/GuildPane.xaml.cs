using DiscordUWA.Common;
using DiscordUWA.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DiscordUWA.UserControls {
    public sealed partial class GuildPane : UserControl {

        public static readonly DependencyProperty ShowUserListProperty = DependencyProperty.Register(
            nameof(ShowUserList),
            typeof(bool),
            typeof(GuildPane),
            new PropertyMetadata(true)
            );

        public bool ShowUserList {
            get { return (bool)GetValue(ShowUserListProperty); }
            set { SetValue(ShowUserListProperty, value); }
        }

        public static readonly DependencyProperty UserListDisplayProperty = DependencyProperty.Register(
            nameof(UserListDisplay),
            typeof(string),
            typeof(GuildPane),
            new PropertyMetadata("Inline")
            );

        public string UserListDisplay {
            get { return (string)GetValue(UserListDisplayProperty); }
            set { SetValue(UserListDisplayProperty, value); }
        }

        public static readonly DependencyProperty ChannelNameProperty = DependencyProperty.Register(
            nameof(ChannelName),
            typeof(string),
            typeof(GuildPane),
            new PropertyMetadata("")
            );

        public string ChannelName {
            get { return (string)GetValue(ChannelNameProperty); }
            set { SetValue(ChannelNameProperty, value); }
        }

        public static readonly DependencyProperty MessageTextProperty = DependencyProperty.Register(
            nameof(MessageText),
            typeof(string),
            typeof(GuildPane),
            new PropertyMetadata("")
            );

        public string MessageText {
            get { return (string)GetValue(MessageTextProperty); }
            set { SetValue(MessageTextProperty, value); }
        }

        public static readonly DependencyProperty SendMessageCommandProperty = DependencyProperty.Register(
            nameof(SendMessageCommand),
            typeof(ICommand),
            typeof(GuildPane),
            new PropertyMetadata(null)
            );

        public ICommand SendMessageCommand {
            get { return (ICommand)GetValue(SendMessageCommandProperty); }
            set { SetValue(SendMessageCommandProperty, value); }
        }

        public static readonly DependencyProperty UserClickCommandProperty = DependencyProperty.Register(
            nameof(UserClick),
            typeof(ICommand),
            typeof(GuildPane),
            new PropertyMetadata(null)
            );

        public ICommand UserClick {
            get { return (ICommand)GetValue(UserClickCommandProperty); }
            set { SetValue(UserClickCommandProperty, value); }
        }

        public static readonly DependencyProperty ServerListToggleCommandProperty = DependencyProperty.Register(
            nameof(ServerListToggle),
            typeof(ICommand),
            typeof(GuildPane),
            new PropertyMetadata(null)
            );

        public ICommand ServerListToggle {
            get { return (ICommand)GetValue(ServerListToggleCommandProperty); }
            set { SetValue(ServerListToggleCommandProperty, value); }
        }

        public static readonly DependencyProperty ChatLogListProperty = DependencyProperty.Register(
            nameof(ChatLogList),
            typeof(IEnumerable<ChatTextListModel>),
            typeof(GuildPane),
            new PropertyMetadata(null)
            );

        public IEnumerable<ChatTextListModel> ChatLogList {
            get { return (IEnumerable<ChatTextListModel>)GetValue(ChatLogListProperty); }
            set { SetValue(ChatLogListProperty, value); }
        }

        public static readonly DependencyProperty ChannelTopicProperty = DependencyProperty.Register(
            nameof(ChannelTopic),
            typeof(string),
            typeof(GuildPane),
            new PropertyMetadata("")
            );

        public string ChannelTopic {
            get { return (string)GetValue(ChannelTopicProperty); }
            set { SetValue(ChannelTopicProperty, value); }
        }

        public static readonly DependencyProperty FullUserListProperty = DependencyProperty.Register(
            nameof(FullUserList),
            typeof(IEnumerable<UserListSectionModel>),
            typeof(UserList),
            new PropertyMetadata(null)
            );

        public IEnumerable<UserListSectionModel> FullUserList {
            get { return (IEnumerable<UserListSectionModel>)GetValue(FullUserListProperty); }
            set { SetValue(FullUserListProperty, value); }
        }

        public static readonly DependencyProperty ToggleUserListCommandProperty = DependencyProperty.Register(
            nameof(ToggleUserListCommand),
            typeof(ICommand),
            typeof(UserList),
            new PropertyMetadata(null)
            );

        public ICommand ToggleUserListCommand {
            get { return (ICommand)GetValue(ToggleUserListCommandProperty); }
            set { SetValue(ToggleUserListCommandProperty, value); }
        }

        public static readonly DependencyProperty PinnedMessagesCommandProperty = DependencyProperty.Register(
            nameof(PinnedMessagesCommand),
            typeof(ICommand),
            typeof(UserList),
            new PropertyMetadata(null)
            );

        public ICommand PinnedMessagesCommand {
            get { return (ICommand)GetValue(PinnedMessagesCommandProperty); }
            set { SetValue(PinnedMessagesCommandProperty, value); }
        }

        public static readonly DependencyProperty LinkClickCommandProperty = DependencyProperty.Register(
            nameof(LinkClickCommand),
            typeof(ICommand),
            typeof(UserList),
            new PropertyMetadata(null)
            );

        public ICommand LinkClickCommand {
            get { return (ICommand)GetValue(LinkClickCommandProperty); }
            set { SetValue(LinkClickCommandProperty, value); }
        }

        public static readonly DependencyProperty AttachmentCommandProperty = DependencyProperty.Register(
            nameof(AttachmentCommand),
            typeof(ICommand),
            typeof(UserList),
            new PropertyMetadata(null)
            );

        public ICommand AttachmentCommand {
            get { return (ICommand)GetValue(AttachmentCommandProperty); }
            set { SetValue(AttachmentCommandProperty, value); }
        }

        // you may be wondering what this is for....
        // its because i cant figure out how to bind textbox 'typing' type update through a usercontrol to a view model
        // this is annoying, but it works
        private void MessageTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            MessageText = MessageTextBox.Text;
        }

        public GuildPane() {
            this.InitializeComponent();
        }
    }
}