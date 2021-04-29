using System;

namespace DataStores
{
    public class BreadOrder : Order
    {
        public BreadOrder(Guid id, string customerName, string customerAddress) : base(id, customerName, customerAddress, GoodsType.Bread)
        {
        }
    }
}