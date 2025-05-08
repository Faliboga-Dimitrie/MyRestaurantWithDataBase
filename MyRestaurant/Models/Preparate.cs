using System;
using System.Collections.Generic;

namespace MyRestaurant.Models;

public partial class Preparate
{
    public int Idpreparat { get; set; }

    public string Denumire { get; set; } = null!;

    public decimal Pret { get; set; }

    public int CantitatePortie { get; set; }

    public int CantitateTotala { get; set; }

    public int Idcategorie { get; set; }

    public virtual ICollection<ComandaPreparat> ComandaPreparats { get; set; } = new List<ComandaPreparat>();

    public virtual ICollection<Fotografi> Fotografis { get; set; } = new List<Fotografi>();

    public virtual Categorii IdcategorieNavigation { get; set; } = null!;

    public virtual ICollection<MeniuPreparat> MeniuPreparats { get; set; } = new List<MeniuPreparat>();

    public virtual ICollection<Alergeni> Idalergens { get; set; } = new List<Alergeni>();
}
