using DiscordUWA.Common;
using DiscordUWA.Models;
using DiscordUWA.ViewModels;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace DiscordUWA.Views {
    public sealed partial class UserProfile : BindablePage {
        public UserProfileViewModel Vm {
            get {
                return (UserProfileViewModel)DataContext;
            }
        }

        public UserProfile() {
            this.InitializeComponent();
        }
    }
}