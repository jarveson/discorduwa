using DiscordUWA.Interfaces;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DiscordUWA.Services {
    public class NavigationService : INavServiceExtend {
        private Frame currentFame;

        private readonly Dictionary<string, Type> pagesByKey = new Dictionary<string, Type>();

        public void SetCurrentFrame(Frame frame) {
            this.currentFame = frame;
        }

        public void Configure(string key, Type pageType) {
            lock (pagesByKey) {
                if (pagesByKey.ContainsKey(key))
                    throw new ArgumentException("This key is already used: " + key);

                if (pagesByKey.Any(p => p.Value == pageType))
                    throw new ArgumentException(
                        "This type is already configured with key " + pagesByKey.First(p => p.Value == pageType).Key);

                pagesByKey.Add(key, pageType);
            }
        }


        public void NavigateTo(string pageKey) {
            NavigateTo(pageKey, null);
        }

        public async void NavigateTo(string pageKey, object parameter) {
            lock (pagesByKey) {
                if (!pagesByKey.ContainsKey(pageKey)) {
                    throw new ArgumentException($"No such page: {pageKey}");
                }
                //currentFame.Navigate(pagesByKey[pageKey], parameter);
            }
            Type page = pagesByKey[pageKey];
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.
           RunAsync(CoreDispatcherPriority.Normal, () => this.currentFame.Navigate(page));
        }

        public void GoBack() {
            if (currentFame.CanGoBack)
                currentFame.GoBack();
        }

        public bool CanGoBack { 
            get {
                var frame = currentFame;
                if (frame != null)
                    return frame.CanGoBack;

                return false;
            }
        }

        public string CurrentPageKey {
            get {
                lock (pagesByKey) {
                    if (currentFame.BackStackDepth == 0)
                        return "root";

                    if (currentFame.Content == null)
                        return "unknown";

                    var currentType = currentFame.Content.GetType();
                    if (pagesByKey.All(p => p.Value != currentType))
                        return "unknown";

                    var item = pagesByKey.FirstOrDefault(i => i.Value == currentType);

                    return item.Key;
                }
            }
        }
    }
}
