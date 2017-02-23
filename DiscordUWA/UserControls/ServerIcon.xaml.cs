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
        public static readonly DependencyProperty GuildNameProperty = DependencyProperty.Register(
            nameof(GuildName),
            typeof(string),
            typeof(ServerIcon),
            new PropertyMetadata("", OnPropertyChangedStatic)
            );

        public string GuildName {
            get { return (string)GetValue(GuildNameProperty); }
            set { SetValue(GuildNameProperty, value); }
        }


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
                string guildAbbr = string.Empty;
                foreach (string s in GuildName.Split(new char[]{' '}, System.StringSplitOptions.RemoveEmptyEntries)) {
                    guildAbbr += char.ToLower(s[0]);
                }
                serverNameText.Text = guildAbbr;
                serverNameText.Visibility = Visibility.Visible;
                serverNameText.IsTextSelectionEnabled = false;
                serverEllipse.Fill = new SolidColorBrush {
                    Color = Windows.UI.Color.FromArgb(0xff, 0x2e, 0x31, 0x36),
                };
                return;
            }
            serverNameText.Visibility = Visibility.Collapsed;

            serverEllipse.Fill = new ImageBrush {
                ImageSource = new BitmapImage {
                    UriSource = new System.Uri(IconUrl),
                    DecodePixelType = DecodePixelType.Logical,
                    DecodePixelHeight = 50,
                    DecodePixelWidth = 50,
                },
            };
        }

        public ServerIcon() {
            this.InitializeComponent();
        }

    }
}