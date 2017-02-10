using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace DiscordUWA.Converters {
    class StringExistsToVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            if (String.IsNullOrEmpty(value as string)) {
                return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
