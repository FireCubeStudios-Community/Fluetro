using Syncfusion.Windows.PdfViewer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Fluetro.Converters
{
    // Converter converts the string into a pageviewmode
    // Base value converter is a base converter that must be included as it provides convert methods
    public class StringToPdfPageViewModeValueConverter : BaseValueConverter<StringToPdfPageViewModeValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, string language)
        {
            //the "value" is the string recieved here we convert it
            if((string)value == "Fit")
            {
                //then we return the proper pageviewmode
                return PageViewMode.FitWidth;
            }
            else if ((string)value == "One page")
            {
                //then we return the proper pageviewmode
                return PageViewMode.OnePage;
            }
            else
            {
                return PageViewMode.Normal;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
