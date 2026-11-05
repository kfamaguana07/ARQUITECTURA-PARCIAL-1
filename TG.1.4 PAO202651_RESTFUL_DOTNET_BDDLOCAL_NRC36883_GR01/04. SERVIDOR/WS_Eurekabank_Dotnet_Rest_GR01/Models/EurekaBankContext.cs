using Microsoft.EntityFrameworkCore;

namespace WS_Eurekabank_Dotnet_Rest_GR01.Models
{
    public class EurekaBankContext : DbContext
    {
        public EurekaBankContext(DbContextOptions<EurekaBankContext> options) : base(options)
        {
        }

        public DbSet<Cuenta> Cuentas { get; set; }
        public DbSet<Movimiento> Movimientos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<TipoMovimiento> TiposMovimiento { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cuenta>().Property(c => c.DecCuenSaldo).HasPrecision(12, 2);
            modelBuilder.Entity<Movimiento>().HasKey(m => new { m.ChrCuenCodigo, m.IntMoviNumero });
            modelBuilder.Entity<Movimiento>().Property(m => m.DecMoviImporte).HasPrecision(12, 2);
            modelBuilder.Entity<Movimiento>()
                .HasOne(m => m.Cuenta)
                .WithMany(c => c.Movimientos)
                .HasForeignKey(m => m.ChrCuenCodigo);
            modelBuilder.Entity<Usuario>().HasKey(u => u.ChrEmplCodigo);
            modelBuilder.Entity<Cliente>().HasKey(cl => cl.ChrClieCodigo);
            modelBuilder.Entity<TipoMovimiento>().HasKey(tm => tm.ChrTipoCodigo);

            base.OnModelCreating(modelBuilder);
        }
    }
}