using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace DiscordUWA.Converters {
    class StringToImageSourceConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            string s = value as string;
            if (String.IsNullOrEmpty(s))
                return null;

            return new BitmapImage(new Uri(s));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            return DependencyProperty.UnsetValue;
        }
    }
}
