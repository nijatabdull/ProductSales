using Microsoft.EntityFrameworkCore;

namespace ProductSale.Services.Order.DAL
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> dbContextOptions) : base(dbContextOptions) { }


        public DbSet<Core.Models.Order> Orders { get; set; }
        public DbSet<Core.Models.OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Core.Models.Order>()
                .ToTable("Orders");

            modelBuilder.Entity<Core.Models.OrderItem>()
                .ToTable("OrderItems");

            modelBuilder.Entity<Core.Models.OrderItem>()
                .Property(x => x.Price).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Core.Models.Order>()
                .OwnsOne(x => x.Address).WithOwner();

            base.OnModelCreating(modelBuilder);
        }
    }
}
