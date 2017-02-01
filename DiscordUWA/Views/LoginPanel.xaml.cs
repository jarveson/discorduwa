using DiscordUWA.Services;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DiscordUWA.Views {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPanel : Page {
        public LoginPanel() {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);
            //if (LocatorService.SettingsService.LocalSettingsContainsKey("loginToken")) {
                textStatusBlock.Text = "Attemtping Token Login....";
                //string token = LocatorService.SettingsService.ReadFromLocalSettings<string>("loginToken");
                string token = "***REMOVED***";
                await LocatorService.DiscordSocketClient.LoginAsync(Discord.TokenType.User, token);
                await LocatorService.DiscordSocketClient.ConnectAsync();
                LocatorService.NavigationService.NavigateTo("main");
                //App.Current.Locator.NavigationService.NavigateFromPageTo(typeof(MainPage));
            //}
        }
    }
}
