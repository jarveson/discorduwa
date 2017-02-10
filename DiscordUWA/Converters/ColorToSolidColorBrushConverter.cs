using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace DiscordUWA.Converters {
    public class ColorToSolidColorBrushValueConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, string language) {
            if (null == value) {
                return null;
            }
            // For a more sophisticated converter, check also the targetType and react accordingly..
            if (value is Color color) {
                color = (Color)value;
                return new SolidColorBrush(color);
            }
            Type type = value.GetType();
            throw new InvalidOperationException("Unsupported type [" + type.Name + "]");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            return (value as SolidColorBrush).Color;
        }
    }
}
