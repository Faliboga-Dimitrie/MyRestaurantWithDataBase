using MyRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyRestaurant.Convertors
{
    public class PreparateConvertor : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null || values.Length < 5)
                return null;

            try
            {
                return new Preparate()
                {
                    Denumire = values[0]?.ToString() ?? string.Empty,
                    Pret = decimal.Parse(values[1]?.ToString() ?? "0"),
                    CantitatePortie = int.Parse(values[2]?.ToString() ?? "0"),
                    CantitateTotala = int.Parse(values[3]?.ToString() ?? "0"),
                    Idcategorie = int.Parse(values[4]?.ToString() ?? "0")
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
