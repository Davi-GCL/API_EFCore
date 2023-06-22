using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace EFCore.Models;

public partial class Conta
{
    public string CodConta { get; set; } = null!;

    public string? Agencia { get; set; }

    public string SetSenha { set 
        {
            var sha = SHA256.Create();
            this.Senha = sha.ComputeHash(Encoding.Default.GetBytes(value));
        } }

    public byte[]? Senha { get; set; }

    public decimal? Saldo { get; set; }

    public byte Tipo { get; set; }

    public int? IdUsuario { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<Mov> Movs { get; set; } = new List<Mov>();
}
