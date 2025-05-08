using System;
using System.Collections.Generic;

namespace MyRestaurant.Models;

public partial class Fotografi
{
    public int Idfotografie { get; set; }

    public int Idpreparat { get; set; }

    public byte[] Fotografie { get; set; } = null!;

    public virtual Preparate IdpreparatNavigation { get; set; } = null!;
}
