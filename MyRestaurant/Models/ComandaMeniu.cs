using System;
using System.Collections.Generic;

namespace MyRestaurant.Models;

public partial class ComandaMeniu
{
    public int Idcomanda { get; set; }

    public int Idmeniu { get; set; }

    public int Cantitate { get; set; }

    public virtual Comenzi IdcomandaNavigation { get; set; } = null!;

    public virtual Meniuri IdmeniuNavigation { get; set; } = null!;
}
