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
        public static void SyncCollection<P, T, K>(this ApplicationDbContext dbContext, P parentObject, List<T> storedItems, List<T> untrackedItems, Func<T, K> getId) where K : IEquatable<K>
        {
            // Remove all stored items that are not present in the untracked collection
            storedItems.RemoveAll(storedItem => !untrackedItems.Any(untrackedItem => getId(storedItem).Equals(getId(untrackedItem))));

            untrackedItems.ForEach(updatedObject =>
            {
                var storedObject = storedItems.SingleOrDefault(stored => getId(updatedObject).Equals(getId(stored)));
                if (storedObject == null)
                {
                    storedItems.Add(updatedObject);
                    dbContext.Entry(parentObject).State = EntityState.Modified;
                    dbContext.Attach(updatedObject).State = EntityState.Added;
                }
                else
                {
                    dbContext.Entry(storedObject).CurrentValues.SetValues(updatedObject);
                }
            });
        }
    }
}
