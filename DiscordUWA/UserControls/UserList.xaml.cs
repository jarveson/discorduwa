using DiscordUWA.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DiscordUWA.UserControls {
    public sealed partial class UserList : UserControl {
        public static DependencyProperty OnlineUserListProperty { get; } = DependencyProperty.Register(
            nameof(OnlineUserList),
            typeof(IEnumerable<UserListModel>),
            typeof(UserList),
            new PropertyMetadata(null)
            );

        public IEnumerable<UserListModel> OnlineUserList {
            get { return (IEnumerable<UserListModel>)GetValue(OnlineUserListProperty); }
            set { SetValue(OnlineUserListProperty, value); }
        }

        public static DependencyProperty OfflineUserListProperty { get; } = DependencyProperty.Register(
            nameof(OfflineUserList),
            typeof(IEnumerable<UserListModel>),
            typeof(UserList),
            new PropertyMetadata(null)
            );

        public IEnumerable<UserListModel> OfflineUserList {
            get { return (IEnumerable<UserListModel>)GetValue(OfflineUserListProperty); }
            set { SetValue(OfflineUserListProperty, value); }
        }

        public UserList() {
            this.InitializeComponent();
        }

    }
}