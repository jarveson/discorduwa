using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace DiscordUWA.Converters {
    class StringNotExistsToVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            if (String.IsNullOrEmpty(value as string)) {
                return Visibility.Visible;
            }
            string temp = (string)value;
            if (temp.Equals("x")) {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
