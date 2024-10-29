using Labb_3.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Labb_3.Converters
{
    internal class IntToDifficulty : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Difficulty.Easy) return 0;
            else if (value is Difficulty.Medium) return 1;
            
            return 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is 0) return Difficulty.Easy;
            else if (value is 1) return Difficulty.Medium;

            return Difficulty.Hard;
        }
    }
}
