using System;
using System.Collections.Generic;

namespace MyRestaurant.Models;

public partial class MeniuPreparat
{
    public int Idmeniu { get; set; }

    public int Idpreparat { get; set; }

    public int CantitateInMeniu { get; set; }

    public virtual Meniuri IdmeniuNavigation { get; set; } = null!;

    public virtual Preparate IdpreparatNavigation { get; set; } = null!;
}
