using System.Threading.Tasks;

namespace DiscordUWA.Interfaces {
    interface INavigable {
        Task OnNavigatedToAsync(object parameter);
        Task OnNavigatedFromAsync(object parameter);
    }
}
