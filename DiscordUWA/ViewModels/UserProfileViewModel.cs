using DiscordUWA.Common;
using DiscordUWA.Interfaces;
using DiscordUWA.Services;
using System;
using System.Windows.Input;
using Windows.UI.Core;

namespace DiscordUWA.ViewModels {
    public class UserProfileViewModel : ViewModelBase {
        private string userName;
        public string UserName {
            get { return this.userName;}
            set { SetProperty(ref userName, value); }
        }

        private string userDescrim;
        public string UserDescrim {
            get { return this.userDescrim;}
            set { SetProperty(ref userDescrim, value); }
        }


        private Windows.UI.Color statusColor;
        public Windows.UI.Color StatusColor {
            get { return this.statusColor; }
            set { SetProperty(ref statusColor, value); }
        }

        private string avatarUrl;
        public string AvatarUrl {
            get { return this.avatarUrl; }
            set { SetProperty(ref avatarUrl, value); }
        }

        public override Task OnNavigatedToAsync(object parameter) {
            var id = parameter as ulong?;
            if (id.HasValue) {
                var currentUser = LocatorService.DiscordSocketClient.GetUser(id.Value);
                avatarUrl = currentUser.AvatarUrl;
                statusColor = currentUser.Status.ToWinColor();
                userName = currentUser.Username;
                UserDescrim = $"#{currentUser.Discriminator}";
            }
            // Add back button to titlebar
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            return Task.CompletedTask;
        }
        public override Task OnNavigatedFromAsync(object parameter) {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            return Task.CompletedTask;
        }

        public UserProfileViewModel() {
        }
    }
}
