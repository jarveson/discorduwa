
using DiscordUWA.Interfaces;
using System.Reflection;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace DiscordUWA.Common {
    public class BindablePage : Page {
        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            var navigableViewModel = this.DataContext as INavigable;
            if (navigableViewModel != null)
                navigableViewModel.OnNavigatingTo(e.Parameter);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            base.OnNavigatedFrom(e);

            var navigableViewModel = this.DataContext as INavigable;
            if (navigableViewModel != null)
                navigableViewModel.OnNavigatingFrom(e.Parameter);
        }
    }
}