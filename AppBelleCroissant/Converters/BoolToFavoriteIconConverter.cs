using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace AppBelleCroissant.Converters
{
    public class BoolToFavoriteIconConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isFavorite)
            {
                return isFavorite ? "heart_filled.png" : "heart_outline.png";
            }
            return "heart_outline.png";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
