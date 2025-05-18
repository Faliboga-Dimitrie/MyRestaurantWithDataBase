using MyRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyRestaurant.Convertors
{
    public class AlergenListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var alergeni = value as ICollection<Alergeni>;
            if (alergeni == null || alergeni.Count == 0)
                return "Fără alergeni";

            return string.Join(", ", alergeni.Select(a => a.NumeAlergen));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
