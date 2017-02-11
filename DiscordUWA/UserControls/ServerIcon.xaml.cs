using DiscordUWA.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace DiscordUWA.UserControls {
    public sealed partial class ServerIcon : UserControl {

        public static readonly DependencyProperty IconUrlProperty = DependencyProperty.Register(
            nameof(IconUrl),
            typeof(string),
            typeof(ServerIcon),
            new PropertyMetadata("", OnPropertyChangedStatic)
            );

        public string IconUrl {
            get { return (string)GetValue(IconUrlProperty); }
            set { SetValue(IconUrlProperty, value); }
        }

        private static void OnPropertyChangedStatic(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var instance = d as ServerIcon;

            // Defer to the instance method.
            instance?.OnPropertyChanged(d, e.Property);
        }

        private void OnPropertyChanged(DependencyObject d, DependencyProperty prop) {
            // rerender the bitmap
            if (string.IsNullOrEmpty(IconUrl)) {
                serverImageBrush.ImageSource = null;
                return;
            }

            serverImageBrush.ImageSource = new BitmapImage {
                UriSource = new System.Uri(IconUrl),
                DecodePixelHeight = 50,
                DecodePixelWidth = 50,
            };
        }

        public ServerIcon() {
            this.InitializeComponent();
        }

    }
}