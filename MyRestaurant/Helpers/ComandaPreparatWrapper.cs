using MyRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRestaurant.Helpers
{
    public class ComandaPreparatWrapper
    {
        public ComandaPreparat ComandaPreparat { get; set; }
        public string PreparatName { get; set; }

        public ComandaPreparatWrapper(ComandaPreparat comandaPreparat, string preparatName)
        {
            ComandaPreparat = comandaPreparat;
            PreparatName = preparatName;
        }
    }
}
