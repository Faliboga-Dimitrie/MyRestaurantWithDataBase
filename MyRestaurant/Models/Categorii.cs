using System;
using System.Collections.Generic;

namespace MyRestaurant.Models;

public partial class Categorii
{
    public int Idcategorie { get; set; }

    public string NumeCategorie { get; set; } = null!;

    public virtual ICollection<Meniuri> Meniuris { get; set; } = new List<Meniuri>();

    public virtual ICollection<Preparate> Preparates { get; set; } = new List<Preparate>();
}
