using Microsoft.EntityFrameworkCore;
using SistemaPedidos.Models;

namespace SistemaPedidos.Data;

/// <summary>
/// Contexto de EF Core: un DbSet por cada entidad del dominio.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Cliente> Clientes { get; set; }

    public DbSet<Pedido> Pedidos { get; set; }

    public DbSet<Producto> Productos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(cliente =>
        {
            cliente.Property(c => c.Nombre).HasMaxLength(100).IsRequired();
            cliente.Property(c => c.Email).HasMaxLength(150).IsRequired();
            cliente.HasIndex(c => c.Email).IsUnique();

            // Cliente → Pedidos (1:N)
            cliente.HasMany(c => c.Pedidos)
                   .WithOne(p => p.Cliente)
                   .HasForeignKey(p => p.ClienteId)
                   .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Producto>(producto =>
        {
            producto.Property(p => p.Nombre).HasMaxLength(100).IsRequired();
            producto.Property(p => p.Precio).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Pedido>(pedido =>
        {
            // Pedido ↔ Producto (N:N), con tabla intermedia PedidoProducto.
            pedido.HasMany(p => p.Productos)
                  .WithMany(pr => pr.Pedidos)
                  .UsingEntity(join => join.ToTable("PedidoProducto"));
        });

        base.OnModelCreating(modelBuilder);
    }
}
