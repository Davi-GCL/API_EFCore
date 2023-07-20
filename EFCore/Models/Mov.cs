using System;
using System.Collections.Generic;

namespace EFCore.Models;

public partial class Mov
{
    public int IdMov { get; set; }

    public int IdConta { get; set; }

    public DateTime? DataHora { get; set; }

    public decimal Valor { get; set; }

    public string Tipo { get; set; } = null!;

    //public virtual Conta IdContaNavigation { get; set; } = null!;
}
