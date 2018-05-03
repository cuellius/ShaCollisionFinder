using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Data;

namespace ShaCollisionFinder
{
    public class CanBuildCollisionsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var files = values.Select(s => s.ToString().Trim()).ToArray();
            return File.Exists(files[0]) && File.Exists(files[1]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}
