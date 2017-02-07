using DiscordUWA.Common;
using DiscordUWA.Interfaces;
using DiscordUWA.Services;
using System;
using System.Windows.Input;

namespace DiscordUWA.ViewModels {
    public class UserProfileViewModel : BindableBase, INavigable {

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

        private Uri avatarUrl;
        public Uri AvatarUrl {
            get { return this.avatarUrl; }
            set { SetProperty(ref avatarUrl, value); }
        }

        public void OnNavigatingTo(object parameter) {
            var id = parameter as ulong?;
            if (id.HasValue) {
                var currentUser = LocatorService.DiscordSocketClient.GetUser(id.Value);
                avatarUrl = new Uri(currentUser.AvatarUrl);
                statusColor = currentUser.Status.ToWinColor();
                userName = currentUser.Username;
                UserDescrim = $"#{currentUser.Discriminator}";
            }
        }
        public void OnNavigatingFrom(object parameter) {

        }

        public UserProfileViewModel() {
        }
    }
}
