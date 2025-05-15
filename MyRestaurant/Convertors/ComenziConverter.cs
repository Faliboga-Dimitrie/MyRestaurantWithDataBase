using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using MyRestaurant.Models;

namespace MyRestaurant.Convertors
{
    public class ComenziConvertor : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null || values.Length < 3)
                return null;

            try
            {
                return new Comenzi()
                {
                    CodUnic = Guid.NewGuid(),              // auto-generate
                    DataComanda = DateTime.Now,            // current date/time
                    Idutilizator = int.Parse(values[0]?.ToString() ?? "0"),
                    Stare = values[1]?.ToString() ?? "Pending",
                    Cost = decimal.Parse(values[2]?.ToString() ?? "0")
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
