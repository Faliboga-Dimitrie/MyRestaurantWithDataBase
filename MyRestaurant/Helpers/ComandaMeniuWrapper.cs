using MyRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRestaurant.Helpers
{
    public class ComandaMeniuWrapper
    {
        public ComandaMeniu ComandaMeniu { get; set; }
        public string MeniuName { get; set; }

        public ComandaMeniuWrapper(ComandaMeniu comandaMeniu, string meniuName)
        {
            ComandaMeniu = comandaMeniu;
            MeniuName = meniuName;
        }
    }
}
