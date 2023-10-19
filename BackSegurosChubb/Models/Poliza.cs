using System;
using System.Collections.Generic;

namespace BackSegurosChubb.Models;

public partial class Poliza
{
    public int IdPoliza { get; set; }

    public string Estado { get; set; } = null!;

    public int IdSeguros { get; set; }

    public int IdAsegurados { get; set; }

    public virtual Persona IdAseguradosNavigation { get; set; } = null!;

    public virtual Seguro IdSegurosNavigation { get; set; } = null!;
}
