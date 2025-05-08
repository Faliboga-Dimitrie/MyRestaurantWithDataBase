using System;
using System.Collections.Generic;

namespace MyRestaurant.Models;

public partial class Comenzi
{
    public int Idcomanda { get; set; }

    public Guid CodUnic { get; set; }

    public int Idutilizator { get; set; }

    public DateTime DataComanda { get; set; }

    public string Stare { get; set; } = null!;

    public decimal Cost { get; set; }

    public virtual ICollection<ComandaMeniu> ComandaMenius { get; set; } = new List<ComandaMeniu>();

    public virtual ICollection<ComandaPreparat> ComandaPreparats { get; set; } = new List<ComandaPreparat>();

    public virtual Utilizatori IdutilizatorNavigation { get; set; } = null!;
}
