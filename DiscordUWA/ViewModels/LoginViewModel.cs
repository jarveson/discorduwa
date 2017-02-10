using DiscordUWA.Common;
using DiscordUWA.Interfaces;
using DiscordUWA.Services;
using System;
using System.Windows.Input;

namespace DiscordUWA.ViewModels {
    public class LoginViewModel : ViewModelBase {

        public ICommand LoginCommand { protected set; get; }

        private string email;
        public string Email {
            get { return this.email; }
            set { SetProperty<string>(ref email, value); }
        }

        private string password;
        public string Password {
            get { return this.password; }
            set { SetProperty<string>(ref password, value); }
        }
        private string errorMessage;
        public string ErrorMessage {
            get { return this.errorMessage; }
            set { SetProperty<string>(ref errorMessage, value); }
        }

        private bool isLoading;
        public bool IsLoading {
            get { return this.isLoading; }
            set { SetProperty<bool>(ref isLoading, value); }
        }
        
        public LoginViewModel() {

            this.LoginCommand = new DelegateCommand(() => {
                this.ErrorMessage = "";
                IsLoading = true;
                try {
                    //string token = await LocatorService.DiscordClient.Connect(Email, Password);
                    //LocatorService.SettingsService.WriteToLocalSettings<string>("loginToken", token);
                    LocatorService.NavigationService.NavigateTo("main");
                }
                catch (Exception ex) {
                    this.ErrorMessage = ex.ToString();
                    //LocatorService.DiscordClient.Log.Error($"LoginError: ", ex);
                }
                IsLoading = false;
            });
        }
    }
}
