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

        public async Task<Model.Order?> GetOrderById(Guid orderId)
        {
            var result = await _dbContext.Orders
                .Include(o => o.OrderLines)
                .SingleOrDefaultAsync(o => o.Id == orderId);

            return result?.ToModel();
        }

        public async Task<Model.Order> UpdateOrder(Model.Order order)
        {
            var current = await _dbContext.Orders
                .Include(o => o.OrderLines)
                .SingleOrDefaultAsync(o => o.Id == order.Id);

            if (current == default)
            {
                throw new ArgumentException($"Cannot update nonexistent order {order.Id}");
            }
            var orderDataContract = order.ToDataContract();
            _dbContext.Entry(current).CurrentValues.SetValues(orderDataContract);

            // Order lines are tracked entities so we need to manually sync the collection
            _dbContext.SyncCollection(current, current.OrderLines, orderDataContract.OrderLines, s => s.Id);


            await _dbContext.SaveChangesAsync();

            return current.ToModel();
        }

        public async Task<Model.Order> CreateOrder(Model.Order order)
        {
            var storedOrderData = _dbContext.Orders.Add(order.ToDataContract());
            await _dbContext.SaveChangesAsync();

            return storedOrderData.Entity.ToModel();
        }
    }
}