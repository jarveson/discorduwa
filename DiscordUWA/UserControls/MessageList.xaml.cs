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
            typeof(IEnumerable<TextTextListModel>),
            typeof(MessageList),
            new PropertyMetadata(null)
            );

        public IEnumerable<ChatTextListModel> TextLogList {
            get { return (IEnumerable<ChatTextListModel>)GetValue(TextLogListProperty); }
            set { SetValue(TextLogListProperty, value); }
        }

        public static readonly DependencyProperty ItemScrollModeProperty = DependencyProperty.Register(
            nameof(ItemScrollMode),
            typeof(ItemsUpdatingScrollMode ),
            typeof(MessageList),
            new PropertyMetadata(default(ItemsUpdatingScrollMode))
            );

        public ItemsUpdatingScrollMode ItemScrollMode {
            get { return (ItemsUpdatingScrollMode)GetValue(ItemScrollModeProperty); }
            set { SetValue(ItemScrollModeProperty, value); }
        }

        public MessageList() {
            this.InitializeComponent();
        }

    }
}