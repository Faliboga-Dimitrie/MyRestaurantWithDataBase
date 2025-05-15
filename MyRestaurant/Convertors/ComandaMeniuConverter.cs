using MyRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyRestaurant.Convertors
{
    public class ComandaMeniuConvertor : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null || values.Length < 3)
                return null;
            try
            {
                return new ComandaMeniu()
                {
                    Idcomanda = int.Parse(values[0]?.ToString() ?? "0"),
                    Idmeniu = int.Parse(values[1]?.ToString() ?? "0"),
                    Cantitate = int.Parse(values[2]?.ToString() ?? "0")
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
