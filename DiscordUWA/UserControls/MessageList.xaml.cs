using DiscordUWA.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace DiscordUWA.UserControls {
    public sealed partial class MessageList : UserControl {
        
        public static readonly DependencyProperty TextLogListProperty = DependencyProperty.Register(
            nameof(TextLogList),
            typeof(IEnumerable<ChatTextListModel>),
            typeof(MessageList),
            new PropertyMetadata(null)
            );

        public IEnumerable<ChatTextListModel> TextLogList {
            get { return (IEnumerable<ChatTextListModel>)GetValue(TextLogListProperty); }
            set { SetValue(TextLogListProperty, value); }
        }

        public static readonly DependencyProperty ItemScrollModeProperty = DependencyProperty.Register(
            nameof(ScrollModeLastItem),
            typeof(bool),
            typeof(MessageList),
            new PropertyMetadata(false)
            );

        public bool ScrollModeLastItem {
            get { return (bool)GetValue(ItemScrollModeProperty); }
            set { SetValue(ItemScrollModeProperty, value); }
        }

        public static readonly DependencyProperty LinkClickCommandProperty = DependencyProperty.Register(
            nameof(LinkClickCommand),
            typeof(ICommand),
            typeof(MessageList),
            new PropertyMetadata(null)
        );

        public ICommand LinkClickCommand {
            get { return (ICommand)GetValue(LinkClickCommandProperty); }
            set { SetValue(LinkClickCommandProperty, value); }
        }

        public async void OnMarkdownUserClick(object sender, LinkClickedEventArgs args) {
            if (LinkClickCommand != null)
                LinkClickCommand.Execute(sender, args);
        }

        public MessageList() {
            this.InitializeComponent();
        }

    }
}