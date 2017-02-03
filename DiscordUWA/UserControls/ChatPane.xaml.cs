using DiscordUWA.Models;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DiscordUWA.UserControls {
    public sealed partial class ChatPane : UserControl {

        public static DependencyProperty ChannelNameProperty { get; } = DependencyProperty.Register(
            nameof(ChannelName),
            typeof(string),
            typeof(ChatPane),
            new PropertyMetadata("")
            );

        public string ChannelName {
            get { return (string)GetValue(ChannelNameProperty); }
            set { SetValue(ChannelNameProperty, value); }
        }

        public static DependencyProperty MessageTextProperty { get; } = DependencyProperty.Register(
            nameof(MessageText),
            typeof(string),
            typeof(ChatPane),
            new PropertyMetadata("")
            );

        public string MessageText {
            get { return (string)GetValue(MessageTextProperty); }
            set { SetValue(MessageTextProperty, value); }
        }

        public static DependencyProperty SendMessageCommandProperty { get; } = DependencyProperty.Register(
            nameof(SendMessageCommand),
            typeof(ICommand),
            typeof(ChatPane),
            new PropertyMetadata(null)
            );

        public ICommand SendMessageCommand {
            get { return (ICommand)GetValue(SendMessageCommandProperty); }
            set { SetValue(SendMessageCommandProperty, value); }
        }

        public static DependencyProperty ChatLogListProperty { get; } = DependencyProperty.Register(
            nameof(ChatLogList),
            typeof(IEnumerable<ChatTextListModel>),
            typeof(ChatPane),
            new PropertyMetadata(null)
            );

        public IEnumerable<ChatTextListModel> ChatLogList {
            get { return (IEnumerable<ChatTextListModel>)GetValue(ChatLogListProperty); }
            set { SetValue(ChatLogListProperty, value); }
        }

        public static DependencyProperty ToggleUserListProperty { get; } = DependencyProperty.Register(
            nameof(ToggleUserList),
            typeof(ICommand),
            typeof(ChatPane),
            new PropertyMetadata(null)
            );

        public ICommand ToggleUserList {
            get { return (ICommand)GetValue(ToggleUserListProperty); }
            set { SetValue(ToggleUserListProperty, value); }
        }

        public ChatPane() {
            this.InitializeComponent();
        }

    }
}