using DiscordUWA.Common;
using DiscordUWA.Interfaces;
using System.Threading.Tasks;

namespace DiscordUWA.ViewModels {
    public abstract class ViewModelBase : BindableBase, INavigable
    {
        public virtual Task OnNavigatedToAsync(object parameter) => Task.CompletedTask;

        public virtual Task OnNavigatedFromAsync(object parameter) => Task.CompletedTask;

    }
}