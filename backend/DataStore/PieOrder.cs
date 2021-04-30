using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStores
{
    public class PieOrder : Order
    {
        public PieOrder(Guid id, string customerName, string customerAddress) : base(id, customerName, customerAddress, GoodsType.Pie)
        {
        }

        public PieOrder(Guid id, string customerName, string customerAddress, List<OrderLine> orderLines) : base(id, customerName, customerAddress, GoodsType.Pie, orderLines)
        {
        }

        public override Model.Order ToModel()
        {
            return new Model.PieOrder()
            {
                Id = Id,
                CustomerName = CustomerName,
                CustomerAddress = CustomerAddress,
                OrderLines = OrderLines.Select(o => o.ToModel()).ToList()
            };
        }
    }
}