using System;
using System.Collections.Generic;

namespace BackSegurosChubb.Models;

public partial class Persona
{
    public int IdAsegurados { get; set; }

    public string Cedula { get; set; } = null!;

    public string NombreCliente { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public int Edad { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<Poliza> Polizas { get; set; } = new List<Poliza>();
}
