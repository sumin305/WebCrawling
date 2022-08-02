using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPFWebSearch.Converter
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var param = parameter.ToString();
            if (param == "ROW")
            {
                var neck = (int)value / 5;
                //if((int)value / 5 == 0)
                //{
                //    return ""
                //}
                return neck;
            }
            else if(param == "COL")
            {
                var namuzi = (int)value % 5;
                return namuzi;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
