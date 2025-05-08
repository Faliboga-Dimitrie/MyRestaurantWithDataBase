using System;
using System.Collections.Generic;

namespace MyRestaurant.Models;

public partial class Utilizatori
{
    public int Idutilizator { get; set; }

    public string Nume { get; set; } = null!;

    public string Prenume { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Telefon { get; set; } = null!;

    public string AdresaLivrare { get; set; } = null!;

    public string Parola { get; set; } = null!;

    public string TipUtilizator { get; set; } = null!;

    public virtual ICollection<Comenzi> Comenzis { get; set; } = new List<Comenzi>();
}
