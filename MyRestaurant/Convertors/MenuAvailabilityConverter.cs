using MyRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MyRestaurant.Convertors
{
    public class MenuAvailabilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var preparate = value as ICollection<MeniuPreparat>;
            if (preparate == null || preparate.Count == 0)
                return Visibility.Visible;

            bool anyIndisponibil = preparate.Any(mp => mp.IdpreparatNavigation?.CantitateTotala <= 0);
            return anyIndisponibil ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
