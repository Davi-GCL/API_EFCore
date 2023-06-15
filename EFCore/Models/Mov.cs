using System;
using System.Collections.Generic;

namespace EFCore.Models;

public partial class Mov
{
    public int IdMov { get; set; }

    public string IdConta { get; set; } = null!;

    public string? DataHora { get; set; }

    public decimal Valor { get; set; }

    public string Tipo { get; set; } = null!;

    public virtual Conta IdContaNavigation { get; set; } = null!;
}
