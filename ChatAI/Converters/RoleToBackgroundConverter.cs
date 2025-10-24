using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace ChatAI.Converters
{
    public class RoleToBackgroundConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string role)
            {
                return role.ToLower() switch
                {
                    "user" => new SolidColorBrush(Colors.LightBlue),
                    "assistant" => new SolidColorBrush(Colors.LightGreen),
                    _ => new SolidColorBrush(Colors.LightGray)
                };
            }
            return new SolidColorBrush(Colors.LightGray);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}