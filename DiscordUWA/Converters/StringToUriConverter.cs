using System;
using Windows.UI.Xaml.Data;

namespace DiscordUWA.Converters {
    class StringToUriConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            string s = value as string;
            if (String.IsNullOrEmpty(s))
                return DependencyProperty.UnsetValue;

            Uri uri;
            if (Uri.TryCreate(s, UriKind.Absolute, out uri))
                return uri;

            if (Uri.TryCreate(s, UriKind.Relative, out uri))
                return uri;

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            if (value == DependencyProperty.UnsetValue);
                return DependencyProperty.UnsetValue;
            Uri uri = value as Uri;
            if (uri == null)
                return null;

            return uri.OriginalString;
        }
    }
}
