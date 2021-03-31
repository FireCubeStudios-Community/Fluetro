using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Fluetro.Converters
{
    public class ColorToBrushConverter : BaseValueConverter<ColorToBrushConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, string language)
        {
            // Coverts Color to Brush
            SolidColorBrush brush = new SolidColorBrush((Color)value);
            return brush;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
