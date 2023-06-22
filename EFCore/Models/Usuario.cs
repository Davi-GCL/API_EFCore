using System;
using System.Collections.Generic;

namespace EFCore.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<Conta> Conta { get; set; } = new List<Conta>();
}
