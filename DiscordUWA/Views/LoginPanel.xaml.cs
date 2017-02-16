using DiscordUWA.Common;
using DiscordUWA.ViewModels;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace DiscordUWA.Views {
    public sealed partial class LoginPanel : BindablePage {
        public LoginViewModel Vm {
            get {
                return (LoginViewModel)DataContext;
            }
        }   

        private void PasswordKeyDown(object sender, KeyRoutedEventArgs e) {
            if (e.Key == Windows.System.VirtualKey.Enter) {
                LoginButton.Focus(FocusState.Programmatic);
                LoginButton.Command.Execute(null);
            }
        }

        public LoginPanel() {
            this.InitializeComponent();
        }
    }
}
