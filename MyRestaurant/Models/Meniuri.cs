using System;
using System.Collections.Generic;

namespace MyRestaurant.Models;

public partial class Meniuri
{
    public int Idmeniu { get; set; }

    public string Denumire { get; set; } = null!;

    public int Idcategorie { get; set; }

    public virtual ICollection<ComandaMeniu> ComandaMenius { get; set; } = new List<ComandaMeniu>();

    public virtual Categorii IdcategorieNavigation { get; set; } = null!;

    public virtual ICollection<MeniuPreparat> MeniuPreparats { get; set; } = new List<MeniuPreparat>();
}
