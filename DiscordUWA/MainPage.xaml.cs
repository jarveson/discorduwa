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

        private void IsPaneOpenPropertyChanged(DependencyObject sender, DependencyProperty dp) {
            if (Vm.ShowUserList)
                UserListSplitView.Width = 300;
            else
                UserListSplitView.Width = 0;
        }

        public MainPage() {
            this.InitializeComponent();

            this.UserListSplitView.RegisterPropertyChangedCallback(SplitView.IsPaneOpenProperty, IsPaneOpenPropertyChanged);
        }
    }
}
