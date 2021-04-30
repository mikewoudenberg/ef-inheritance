using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStores
{
    public class BreadOrder : Order
    {
        public BreadOrder(Guid id, string customerName, string customerAddress) : base(id, customerName, customerAddress, GoodsType.Bread)
        {
        }

        public BreadOrder(Guid id, string customerName, string customerAddress, List<OrderLine> orderLines) : base(id, customerName, customerAddress, GoodsType.Bread, orderLines)
        {
        }

        public override Model.Order ToModel()
        {
            return new Model.BreadOrder()
            {
                Id = Id,
                CustomerName = CustomerName,
                CustomerAddress = CustomerAddress,
                OrderLines = OrderLines.Select(o => o.ToModel()).ToList()
            };
        }
    }
}