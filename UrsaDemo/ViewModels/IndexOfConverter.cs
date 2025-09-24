using Avalonia.Data.Converters;
using System;
using System.Globalization;
using System.Linq;

namespace UrsaDemo.ViewModels
{
    public class IndexOfConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is System.Collections.IEnumerable collection && value != null)
            {
                int index = 0;
                foreach (var item in collection)
                {
                    if (item == value)
                        return index;
                    index++;
                }
            }
            return -1; // 未找到时返回 -1
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("ConvertBack is not supported.");
        }
    }
}