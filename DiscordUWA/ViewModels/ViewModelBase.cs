using System.Collections.Generic;
using System.Threading.Tasks;
//using Newtonsoft.Json;
using Windows.UI.Xaml.Navigation;

namespace DiscordUWA.ViewModels {
    public abstract class ViewModelBase : BindableBase, INavigable
    {
        public virtual Task OnNavigatedToAsync(object parameter) => Task.CompletedTask;

        public virtual Task OnNavigatedFromAsync() => Task.CompletedTask;

    }
}