using DiscordUWA.Common;
using DiscordUWA.Interfaces;
using DiscordUWA.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Data.Json;
using Windows.Web.Http;

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

            this.LoginCommand = new DelegateCommand(async () => {
                this.ErrorMessage = "";
                IsLoading = true;
                try {
                    JsonObject loginContent = new JsonObject();
                    loginContent.SetNamedValue("email", JsonValue.CreateStringValue(Email));
                    loginContent.SetNamedValue("password", JsonValue.CreateStringValue(Password));

                    using (HttpClient httpClient = new HttpClient()) {
                        HttpResponseMessage response = await httpClient.PostAsync(
                            new Uri("https://discordapp.com/api/v6/auth/login"),
                            new HttpStringContent(loginContent.Stringify(), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"));
                        if (response.StatusCode != HttpStatusCode.Ok) {
                            ErrorMessage = "Login Failed";
                            return;
                        }
                        JsonObject jsonresp = JsonObject.Parse(response.Content.ToString());
                        string token = jsonresp.GetNamedString("token", "");
                        LocatorService.SecretService.WriteSecret(SettingKeys.Token, token);
                    }
                    await AttemptTokenLogin();
                }
                catch (Exception ex) {
                    this.ErrorMessage = ex.ToString();
                    //LocatorService.DiscordClient.Log.Error($"LoginError: ", ex);
                }
                finally {
                    IsLoading = false;
                }
            });
        }

        private async Task AttemptTokenLogin() {
            string token = LocatorService.SecretService.ReadSecret(SettingKeys.Token);
            if (token != string.Empty) {
                this.ErrorMessage = "Attempting Token Login....";
                try {
                    IsLoading = true;
                    await LocatorService.DiscordSocketClient.LoginAsync(Discord.TokenType.User, token);
                    await LocatorService.DiscordSocketClient.ConnectAsync();
                    LocatorService.NavigationService.NavigateTo("main");
                }
                catch (Exception) {
                    this.ErrorMessage = "Token login failed.";
                }
                finally {
                    IsLoading = false;
                }
            }
            IsLoading = false;
        }
    }
}
