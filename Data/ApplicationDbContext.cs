using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GestaoFrota.Models;
public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
    public DbSet<Motorista> Motoristas { get; set; }
    public DbSet<Veiculo> Veiculos { get; set; }
    public DbSet<Manutencao> Manutencoes { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Manutencao>()
            .Property(m => m.Custo)
            .HasPrecision(18, 2);
        modelBuilder.Entity<Veiculo>()
            .ToTable(tb => tb.HasTrigger("trg_DeleteVeiculo"));
    }
}
