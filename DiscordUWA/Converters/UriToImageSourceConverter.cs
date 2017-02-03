using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace DiscordUWA.Converters {
    class UriToImageSourceConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            Uri s = value as Uri;
            if (s == null)
                return null;

            return new BitmapImage(s);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            BitmapImage image = value as BitmapImage;
            if (image == null)
                return null;

            return image.UriSource;
        }
    }
}
