using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using MyRestaurant.Models;

namespace MyRestaurant.Convertors
{
    internal class UtilizatoriConvertor : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(values == null || values.Length < 7)
                return null;

            try
            {
                return new Utilizatori()
                {
                    Nume = values[0]?.ToString() ?? string.Empty,
                    Prenume = values[1]?.ToString() ?? string.Empty,
                    Email = values[2]?.ToString() ?? string.Empty,
                    Telefon = values[3]?.ToString() ?? string.Empty,
                    AdresaLivrare = values[4]?.ToString() ?? string.Empty,
                    Parola = values[5]?.ToString() ?? string.Empty,
                    TipUtilizator = values[6]?.ToString() ?? string.Empty
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
