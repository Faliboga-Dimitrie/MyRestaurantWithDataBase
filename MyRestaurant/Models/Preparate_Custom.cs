using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRestaurant.Models
{
    public partial class Preparate
    {
        /// <summary>
        /// Read-only property to get the category name from the navigation property.
        /// Returns "N/A" if the category is not loaded.
        /// </summary>
        public string CategoryName => IdcategorieNavigation?.NumeCategorie ?? "N/A";
    }
}
