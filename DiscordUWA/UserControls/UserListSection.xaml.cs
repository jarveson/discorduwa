using DiscordUWA.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DiscordUWA.UserControls {
    public sealed partial class UserListSection : UserControl {
        public static DependencyProperty UserListProperty { get; } = DependencyProperty.Register(
            nameof(UserList),
            typeof(IEnumerable<UserListModel>),
            typeof(UserList),
            new PropertyMetadata(null)
            );

        public IEnumerable<UserListModel> UserList {
            get { return (IEnumerable<UserListModel>)GetValue(UserListProperty); }
            set { SetValue(UserListProperty, value); }
        }

        public static DependencyProperty RoleNameProperty { get; } = DependencyProperty.Register(
            nameof(RoleName),
            typeof(string),
            typeof(UserList),
            new PropertyMetadata(null)
            );

        public string RoleName {
            get { return (string)GetValue(RoleNameProperty); }
            set { SetValue(RoleNameProperty, value); }
        }

        public UserListSection() {
            this.InitializeComponent();
        }
    }
}
