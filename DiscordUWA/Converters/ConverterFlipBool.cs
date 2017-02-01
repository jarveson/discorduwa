using System;
using Windows.UI.Xaml.Data;

namespace DiscordUWA.Converters {
    class ConverterFlipBool : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string culture) {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture) {
            return !(bool)value;
        }
    }

}
