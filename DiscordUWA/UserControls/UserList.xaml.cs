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
    public sealed partial class UserList : UserControl {
        public static DependencyProperty GuildIdProperty { get; } = DependencyProperty.Register(
            nameof(ChannelId),
            typeof(long),
            typeof(ChatPane),
            new PropertyMetadata(null, new PropertyChangedCallback(OnSourcePropertyChanged))
            );

        public long GuildId {
            get { return (long)GetValue(UriSourceProperty); }
            set { SetValue(UriSourceProperty, value); }
        }

        public static DependencyProperty OnlineUserListProperty { get; } = DependencyProperty.Register(
            nameof(OnlineUserList),
            typeof(IEnumerable<UserListModel>),
            typeof(ChatPane),
            new PropertyMetadata(null, new PropertyChangedCallback(OnSourcePropertyChanged))
            );

        public IEnumerable<UserListModel> OnlineUserList {
            get { return (IEnumerable<UserListModel>)GetValue(UriSourceProperty); }
            set { SetValue(UriSourceProperty, value); }
        }

        public static DependencyProperty OfflineUserListProperty { get; } = DependencyProperty.Register(
            nameof(OfflineUserList),
            typeof(IEnumerable<UserListModel>),
            typeof(ChatPane),
            new PropertyMetadata(null, new PropertyChangedCallback(OnSourcePropertyChanged))
            );

        public IEnumerable<UserListModel> OfflineUserList {
            get { return (IEnumerable<UserListModel>)GetValue(UriSourceProperty); }
            set { SetValue(UriSourceProperty, value); }
        }

    }
}