using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using AvaloniaPrsimSimple.ViewModels;

namespace AvaloniaPrsimSimple.Models;

public class LightStatusToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is LightStatus status)
        {
            switch (status)
            {
                case LightStatus.Off:
                    return Brushes.Gray;
                case LightStatus.Red:
                    return Brushes.Red;
                case LightStatus.Green:
                    return Brushes.Green;
                case LightStatus.Blue:
                    return Brushes.Blue;
            }
        }
        return Brushes.Gray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}