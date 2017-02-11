using DiscordUWA.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace DiscordUWA.UserControls {
    public sealed partial class UserIcon : UserControl {

        public static readonly DependencyProperty IconUrlProperty = DependencyProperty.Register(
            nameof(IconUrl),
            typeof(string),
            typeof(UserIcon),
            new PropertyMetadata("", OnPropertyChangedStatic)
            );

        public string IconUrl {
            get { return (string)GetValue(IconUrlProperty); }
            set { SetValue(IconUrlProperty, value); }
        }

        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(
            nameof(IconSize),
            typeof(string),
            typeof(UserIcon),
            new PropertyMetadata(30, OnPropertyChangedStatic)
            );

        public int IconSize {
            get { return (int)GetValue(IconSizeProperty); }
            set { SetValue(IconSizeProperty, value); }
        }

        private static void OnPropertyChangedStatic(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var instance = d as UserIcon;

            // Defer to the instance method.
            instance?.OnPropertyChanged(d, e.Property);
        }

        private void OnPropertyChanged(DependencyObject d, DependencyProperty prop) {
            // rerender the bitmap
            if (string.IsNullOrEmpty(IconUrl)) {
                iconImageBrush.ImageSource = null;
                return;
            }
            iconImageBrush.ImageSource = new BitmapImage {
                UriSource = new System.Uri(IconUrl),
                DecodePixelHeight = IconSize,
                DecodePixelWidth = IconSize,
            };
        }

        public UserIcon() {
            this.InitializeComponent();
        }

    }
}