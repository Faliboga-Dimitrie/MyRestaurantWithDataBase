using System;
using System.Collections.Generic;

namespace MyRestaurant.Models;

public partial class ComandaPreparat
{
    public int Idcomanda { get; set; }

    public int Idpreparat { get; set; }

    public int Cantitate { get; set; }

    public virtual Comenzi IdcomandaNavigation { get; set; } = null!;

    public virtual Preparate IdpreparatNavigation { get; set; } = null!;
}
