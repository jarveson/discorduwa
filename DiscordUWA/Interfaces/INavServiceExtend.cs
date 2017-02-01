using GalaSoft.MvvmLight.Views;
using Windows.UI.Xaml.Controls;

namespace DiscordUWA.Interfaces {
    public interface INavServiceExtend : INavigationService {
        void SetCurrentFrame(Frame frame);
    }
}
