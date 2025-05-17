using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using MyRestaurant.Models;

namespace MyRestaurant.Convertors
{
    public class FotografiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null || values.Length < 2)
                return null;

            try
            {
                int idpreparat = 0;
                if (!int.TryParse(values[0]?.ToString(), out idpreparat))
                    return null;

                var imageBytes = values[1] as byte[];
                if (imageBytes == null || imageBytes.Length == 0)
                    return null;

                return new Fotografi()
                {
                    Idpreparat = idpreparat,
                    Fotografie = imageBytes
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
