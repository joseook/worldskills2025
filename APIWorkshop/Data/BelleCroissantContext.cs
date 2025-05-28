using APIWorkshop.Model;
using Microsoft.EntityFrameworkCore;

namespace APIWorkshop.Data {
    public class BelleCroissantContext : DbContext {

        public DbSet<Product> Products {get; set; }
        public DbSet<Customer> Customers {get; set; }
        public DbSet<Order> Orders {get; set; }
        
        public BelleCroissantContext(DbContextOptions<BelleCroissantContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // Chave primária
           modelBuilder.Entity<Order>()
                .HasKey(i => i.TransactionId);


            // Configurando relacionamento entre pedidos/vendas e clientes
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(k => k.CustomerId);


            // Configurando relacionamento entre pedidos/vendas e produtos
            modelBuilder.Entity<Order>() 
                .HasOne(o => o.Product)
                .WithMany()
                .HasForeignKey(k => k.ProductId);

        }
    }
}
