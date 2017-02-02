using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Graphics.DirectX;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DiscordUWA.UserControls {
    public sealed partial class ChatPane : UserControl {
        public static DependencyProperty ChannelIdProperty { get; } = DependencyProperty.Register(
            nameof(ChannelId),
            typeof(long),
            typeof(ChatPane),
            new PropertyMetadata(null, new PropertyChangedCallback(OnSourcePropertyChanged))
            );

        public long ChannelId {
            get { return (long)GetValue(ChannelIdProperty); }
            set { SetValue(ChannelIdProperty, value); }
        }

        public static DependencyProperty ChannelNameProperty { get; } = DependencyProperty.Register(
            nameof(ChannelName),
            typeof(string),
            typeof(ChatPane),
            new PropertyMetadata(null, new PropertyChangedCallback(OnSourcePropertyChanged))
            );

        public string ChannelName {
            get { return (string)GetValue(ChannelNameProperty); }
            set { SetValue(ChannelNameProperty, value); }
        }

        public static DependencyProperty MessageTextProperty { get; } = DependencyProperty.Register(
            nameof(MessageText),
            typeof(string),
            typeof(ChatPane),
            new PropertyMetadata(null, new PropertyChangedCallback(OnSourcePropertyChanged))
            );

        public string MessageText {
            get { return (string)GetValue(MessageTextProperty); }
            set { SetValue(MessageTextProperty, value); }
        }

        public static DependencyProperty SendMessageCommandProperty { get; } = DependencyProperty.Register(
            nameof(SendMessageCommand),
            typeof(ICommand),
            typeof(ChatPane),
            new PropertyMetadata(null, new PropertyChangedCallback(OnSourcePropertyChanged))
            );

        public ICommand SendMessageCommand {
            get { return (ICommand)GetValue(SendMessageCommandProperty); }
            set { SetValue(SendMessageCommandProperty, value); }
        }

        public static DependencyProperty ChatLogListProperty { get; } = DependencyProperty.Register(
            nameof(ChatLogList),
            typeof(IEnumerable<ChatTextListModel>),
            typeof(ChatPane),
            new PropertyMetadata(null, new PropertyChangedCallback(OnSourcePropertyChanged))
            );

        public IEnumerable<ChatTextListModel> ChatLogList {
            get { return (IEnumerable<ChatTextListModel>)GetValue(ChatLogListProperty); }
            set { SetValue(ChatLogListProperty, value); }
        }
    }
}