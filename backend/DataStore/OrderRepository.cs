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

        public Task<Model.Order?> GetOrderById(Guid orderId)
        {
            var result = _dbContext.Orders
                .Include(o => o.OrderLines)
                .SingleOrDefault(o => o.Id == orderId);
            if (result == default)
            {
                result = null;
            }
            return Task.FromResult(result?.ToModel());
        }

        public Task<Model.Order> UpdateOrder(Model.Order order)
        {
            var current = _dbContext.Orders
                .Include(o => o.OrderLines)
                .SingleOrDefault(o => o.Id == order.Id);

            if (current == default)
            {
                throw new ArgumentException($"Cannot update nonexistent order {order.Id}");
            }
            var orderDataContract = order.ToDataContract();
            _dbContext.Entry(current).CurrentValues.SetValues(orderDataContract);

            // Order lines are tracked entities so we need to manually sync the collection
            _dbContext.SyncCollection(current.OrderLines, orderDataContract.OrderLines, s => s.Id);


            _dbContext.SaveChanges();

            return Task.FromResult(current.ToModel());
        }

        public Task<Model.Order> CreateOrder(Model.Order order)
        {
            var storedOrderData = _dbContext.Orders.Add(order.ToDataContract());
            _dbContext.SaveChanges();

            return Task.FromResult(storedOrderData.Entity.ToModel());
        }
    }
}