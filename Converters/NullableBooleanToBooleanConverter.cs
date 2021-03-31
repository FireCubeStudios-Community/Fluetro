using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluetro.Converters
{
    public class NullableBooleanToBooleanConverter : BaseValueConverter<NullableBooleanToBooleanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, string language)
        {
            bool boolean;
            if(value == null)
            {
                boolean = false;
            }
            else
            {
                boolean = (bool)value;
            }
            return boolean;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
