using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataStores
{

    public class OrderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Order?> GetOrderById(Guid orderId)
        {
            var result = _dbContext.Orders
                .Include(o => o.OrderLines)
                .SingleOrDefault(o => o.Id == orderId);
            if (result == default)
            {
                result = null;
            }
            return Task.FromResult(result);
        }

        public Task<Order> UpdateOrder(Order order)
        {
            var current = _dbContext.Orders
                .Include(o => o.OrderLines)
                .SingleOrDefault(o => o.Id == order.Id);

            if (current == default)
            {
                throw new ArgumentException($"Cannot update nonexistent order {order.Id}");
            }

            _dbContext.Entry(current).CurrentValues.SetValues(order);

            // Order lines are tracked entities so we need to manually sync the collection
            _dbContext.SyncCollection(current.OrderLines, order.OrderLines, s => s.Id);


            _dbContext.SaveChanges();

            return Task.FromResult(current);
        }

        public Task<Order> CreateOrder(Order order)
        {
            var storedOrderData = _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();

            return Task.FromResult(storedOrderData.Entity);
        }
    }
}