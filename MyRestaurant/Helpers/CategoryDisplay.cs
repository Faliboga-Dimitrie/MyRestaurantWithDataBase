using MyRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRestaurant.Helpers
{
    public class CategoryDisplay
    {
        public string NumeCategorie { get; set; }
        public ObservableCollection<Preparate> Preparates { get; set; }
        public ObservableCollection<Meniuri> Meniuris { get; set; }
    }
}
