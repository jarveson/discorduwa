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
        
        public override async Task OnNavigatedToAsync(object parameter) {
            await AttemptTokenLogin();
        }

        public LoginViewModel() {

            this.LoginCommand = new DelegateCommand(() => {
                this.ErrorMessage = "";
                IsLoading = true;
                try {
                    string content = $"\{email:\"{Email}\",password: \"{Password}\"\}";
                    JsonObject loginContent = new JsonObject();
                    loginContent.SetNamedValue("email", Email);
                    loginContent.SetNamedValue("password", Password);

                    HttpResponseMessage response = await httpClient.PostAsync(
                        new Uri("https://discordapp.com/api/v6/auth/login"),
                        new HttpStringContent(loginContent.Stringify()));

                    if (response.StatusCode != HttpStatusCode.OK) {
                        ErrorMessage = "Login Failed";
                        return;
                    } 
                    JsonObject jsonresp = JsonObject.Parse(response.Content);
                    string token = jsonresp.GetNamedString("token", "");
                    LocatorService.SecretService.WriteSecret("token", token);
                    AttemptTokenLogin();
                }
                catch (Exception ex) {
                    this.ErrorMessage = ex.ToString();
                    //LocatorService.DiscordClient.Log.Error($"LoginError: ", ex);
                }
                IsLoading = false;
            });
        }

        private async Task AttemptTokenLogin() {
            string token = LocatorService.SecretService.ReadSecret("Token");
            if (token != string.Empty) {
                textStatusBlock.Text = "Attempting Token Login....";
                try {
                    await LocatorService.DiscordSocketClient.LoginAsync(Discord.TokenType.User, token);
                    await LocatorService.DiscordSocketClient.ConnectAsync();
                    LocatorService.NavigationService.NavigateTo("main");
                }
                catch (Exception ex) {
                    this.ErrorMessage = "Token login failed.";
                }
            }
        }
    }
}
