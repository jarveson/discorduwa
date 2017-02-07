using DiscordUWA.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DiscordUWA.UserControls {
    public sealed partial class UserList : UserControl {
        public static DependencyProperty UserListProperty { get; } = DependencyProperty.Register(
            nameof(FullUserList),
            typeof(IEnumerable<UserListSectionModel>),
            typeof(UserList),
            new PropertyMetadata(null)
            );

        public IEnumerable<UserListSectionModel> FullUserList {
            get { return (IEnumerable<UserListSectionModel>)GetValue(UserListProperty); }
            set { SetValue(UserListProperty, value); }
        }

        public static DependencyProperty UserClickProperty { get; } = DependencyProperty.Register(
            nameof(UserClick),
            typeof(ICommand),
            typeof(UserList),
            new PropertyMetadata(null)
            );

        public ICommand UserClick {
            get { return (ICommand)GetValue(UserClickProperty); }
            set { SetValue(UserClickProperty, value); }
        }

        public static DependencyProperty ShowChannelTopicProperty { get; } = DependencyProperty.Register(
            nameof(ShowChannelTopic),
            typeof(bool),
            typeof(UserList),
            new PropertyMetadata(null)
            );

        public bool ShowChannelTopic {
            get { return (bool)GetValue(ShowChannelTopicProperty); }
            set { SetValue(ShowChannelTopicProperty, value); }
        }

        public static DependencyProperty ChannelTopicProperty { get; } = DependencyProperty.Register(
            nameof(ChannelTopic),
            typeof(string),
            typeof(UserList),
            new PropertyMetadata(null)
            );

        public string ChannelTopic {
            get { return (string)GetValue(ChannelTopicProperty); }
            set { SetValue(ChannelTopicProperty, value); }
        }

        private void OnItemClick(object sender, ItemClickEventArgs e) {
            var selected = e.ClickedItem as UserListSectionModel;
            UserClick.Execute(selected.Id);
        }

        public UserList() {
            this.InitializeComponent();
        }

    }
}