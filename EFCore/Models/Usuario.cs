using EFCore.Services;
using System;
using System.Collections.Generic;

namespace EFCore.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string? Email { get; set; } = null!;

    public string? Telefone { get; set; } = null!;

    public string? Cpf { get; set; } = null!;

    public string SetSenha
    {
        set
        {
            Senha = value.GerarHash();
        }
    }

    public string? Senha { get; set; }

    public virtual ICollection<Conta> Conta { get; set; } = new List<Conta>();
}
