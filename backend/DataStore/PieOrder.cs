using System;

namespace DataStores
{
    public class PieOrder : Order
    {
        public PieOrder(Guid id, string customerName, string customerAddress) : base(id, customerName, customerAddress, GoodsType.Pie)
        {
        }
    }
}