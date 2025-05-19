using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRestaurant.Helpers
{
    public class CostCalculationResult
    {
        public decimal FinalCost { get; set; }
        public List<string> Messages { get; } = new List<string>();
    }
}
