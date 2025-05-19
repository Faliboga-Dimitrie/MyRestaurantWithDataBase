using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Xml;

namespace MyRestaurant.Helpers
{
    public class DiscountSettings
    {
        public decimal DiscountOrderThreshold { get; }
        public int MaxOrdersInInterval { get; }
        public TimeSpan OrderInterval { get; }
        public decimal DiscountPercent { get; }
        public decimal FreeDeliveryThreshold { get; }
        public decimal DeliveryFee { get; }

        public DiscountSettings()
        {
            DiscountOrderThreshold = decimal.Parse(ConfigurationManager.AppSettings["DiscountOrderThreshold"]);
            MaxOrdersInInterval = int.Parse(ConfigurationManager.AppSettings["MaxOrdersInInterval"]);

            try
            {
                OrderInterval = System.Xml.XmlConvert.ToTimeSpan(ConfigurationManager.AppSettings["OrderInterval"]);
            }
            catch
            {
                OrderInterval = TimeSpan.FromHours(1); // default fallback
            }

            DiscountPercent = decimal.Parse(ConfigurationManager.AppSettings["DiscountPercent"]);
            FreeDeliveryThreshold = decimal.Parse(ConfigurationManager.AppSettings["FreeDeliveryThershold"]);
            DeliveryFee = decimal.Parse(ConfigurationManager.AppSettings["DeliveryFee"]);
        }
    }

}
