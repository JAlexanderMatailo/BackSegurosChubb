using System;
using System.Collections.Generic;

namespace BackSegurosChubb.Models;

public partial class Seguro
{
    public int IdSeguros { get; set; }

    public string NombreSeguro { get; set; } = null!;

    public string Codigo { get; set; } = null!;

    public double SumaAsegurada { get; set; }

    public double Prima { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<Poliza> Polizas { get; set; } = new List<Poliza>();
}
