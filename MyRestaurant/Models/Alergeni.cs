using System;
using System.Collections.Generic;

namespace MyRestaurant.Models;

public partial class Alergeni
{
    public int Idalergen { get; set; }

    public string NumeAlergen { get; set; } = null!;

    public virtual ICollection<Preparate> Idpreparats { get; set; } = new List<Preparate>();
}
