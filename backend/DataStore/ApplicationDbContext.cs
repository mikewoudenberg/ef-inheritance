using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DataStores
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasDiscriminator(o => o.GoodsType)
                .HasValue<BreadOrder>(GoodsType.Bread)
                .HasValue<PieOrder>(GoodsType.Pie);
            modelBuilder.Entity<OrderLine>()
                .HasDiscriminator(o => o.GoodsType)
                .HasValue<PieOrderLine>(GoodsType.Pie)
                .HasValue<BreadOrderLine>(GoodsType.Bread);
        }

        public DbSet<Order> Orders { get; private set; } = null!; // this field is set through entity framework
    }

    public static class DbContextExtensions
    {
        public static void SyncCollection<T, K>(this ApplicationDbContext dbContext, List<T> stored, List<T> updated, Func<T, K> getId) where K : IEquatable<K>
        {
            stored.RemoveAll(storedObject => !updated.Any(updatedObject => getId(storedObject).Equals(getId(updatedObject))));
            updated.ForEach(updatedObject =>
            {
                var storedObject = stored.SingleOrDefault(specificProduct => getId(updatedObject).Equals(getId(specificProduct)));
                if (storedObject == null)
                {
                    // Ensure the added entity is seen as added and not tracked already
                    dbContext.Entry(updatedObject).State = EntityState.Added;
                    stored.Add(updatedObject);
                }
                else
                {
                    dbContext.Entry(storedObject).CurrentValues.SetValues(updatedObject);
                }
            });
        }
    }
}
