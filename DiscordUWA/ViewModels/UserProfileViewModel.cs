using DiscordUWA.Common;
using DiscordUWA.Interfaces;
using DiscordUWA.Services;
using System;
using System.Windows.Input;

namespace DiscordUWA.ViewModels {
    public class UserProfileViewModel : INavigable, BindableBase {

        public void OnNavigatingTo(object parameter) {
            //todo: load stuff from username?
        }
        public void OnNavigatingFrom(object parameter) {

        }

        public UserProfileViewModel() {
        }
    }
}
