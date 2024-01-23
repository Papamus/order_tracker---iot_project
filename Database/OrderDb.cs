using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OrderTracker.Database.Entities;

namespace OrderTracker.Database
{
    public class OrderDb: DbContext
    {
        public OrderDb(DbContextOptions<OrderDb> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           ConfigureOrderEntity(modelBuilder.Entity<OrderEntity>());
        }

        private void ConfigureOrderEntity(EntityTypeBuilder<OrderEntity> entity)
        {
            entity.ToTable("Order");
            entity.Property(p => p.CustomerName).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Status).IsRequired().HasConversion(new EnumToStringConverter<OrderStatus>());
            entity.Property(p => p.Price).IsRequired();
        }

        public DbSet<OrderEntity> Orders{get;set;}
    }


}