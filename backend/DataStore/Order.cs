using System;
using System.Collections.Generic;

namespace DataStores
{
    public abstract class Order
    {
        // Ctor for entityframework
        protected Order(Guid id, string customerName, string customerAddress, GoodsType goodsType)
        {
            Id = id;
            CustomerName = customerName;
            CustomerAddress = customerAddress;
            GoodsType = goodsType;
        }

        public Order(Guid id, string customerName, string customerAddress, GoodsType goodsType, List<OrderLine> orderLines) : this(id, customerName, customerAddress, goodsType)
        {
            Id = id;
            OrderLines = orderLines;
        }
        public Guid Id { get; private set; }
        public string CustomerName { get; private set; }
        public string CustomerAddress { get; private set; }
        public GoodsType GoodsType { get; private set; }
        public List<OrderLine> OrderLines { get; } = new List<OrderLine>();
    }
}