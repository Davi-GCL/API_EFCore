using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Models;

public partial class SistemaBancoContext : DbContext
{
    public SistemaBancoContext()
    {
    }

    public SistemaBancoContext(DbContextOptions<SistemaBancoContext> options)
        : base(options)
    {
        Database.EnsureCreated(); //Metodo que verifica se já existe uma base de dados, se não existe, ele cria uma;
    }

//Mapeamento das entidades (é necessario uma prop DbSet para cada entidade) "DbSet<classe>":
    public virtual DbSet<Conta> Contas { get; set; }

    public virtual DbSet<Mov> Movs { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)=> optionsBuilder.UseSqlServer("Password=root;Persist Security Info=True;User ID=sa;Initial Catalog=sistema_banco;Data Source=LUNA-PC\\SQLEXPRESS;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Conta>(entity =>
        {
            entity.HasKey(e => e.CodConta).HasName("PK__contas__B89B358CE048E355");

            entity.ToTable("contas");

            entity.Property(e => e.CodConta)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("codConta");

            entity.Property(e => e.Agencia)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("agencia");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Saldo)
                .HasDefaultValueSql("((0))")
                .HasColumnType("money")
                .HasColumnName("saldo");
            entity.Property(e => e.Senha)
                .HasMaxLength(100)
                .HasColumnName("senha");
            entity.Property(e => e.Tipo).HasColumnName("tipo");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Conta)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__contas__idUsuari__4CA06362");
        });

        modelBuilder.Entity<Mov>(entity =>
        {
            entity.HasKey(e => e.IdMov).HasName("PK__mov__3DC69A4F779BE56D");

            entity.ToTable("mov");

            entity.Property(e => e.IdMov).HasColumnName("idMov");
            entity.Property(e => e.DataHora)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("dataHora");
            entity.Property(e => e.IdConta)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("idConta");
            entity.Property(e => e.Tipo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tipo");
            entity.Property(e => e.Valor)
                .HasColumnType("money")
                .HasColumnName("valor");

            //entity.HasOne(d => d.IdContaNavigation).WithMany(p => p.Movs)
            //    .HasForeignKey(d => d.IdConta)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__mov__idConta__571DF1D5");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__usuarios__3213E83F7722FE27");

            entity.ToTable("usuarios");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nome");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
