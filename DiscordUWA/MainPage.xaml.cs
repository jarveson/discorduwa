using DiscordUWA.Common;
using DiscordUWA.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DiscordUWA {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : BindablePage {
        public ServerViewModel Vm {
            get {
                return (ServerViewModel)DataContext;
            }
        }

        public MainPage() {
            this.InitializeComponent();
        }
    }
}
