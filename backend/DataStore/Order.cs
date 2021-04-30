using System;
using System.Collections.Generic;
using System.Linq;

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

        public abstract Model.Order ToModel();
    }

    public static class OrderExtensions
    {
        public static Order ToDataContract(this Model.Order order)
        {
            return order switch
            {
                Model.BreadOrder breadOrder =>
                    new BreadOrder
                        (
                            breadOrder.Id,
                            breadOrder.CustomerName,
                            breadOrder.CustomerAddress,
                            order.OrderLines.Select(o => o.ToDataContract()).ToList()
                        ),
                Model.PieOrder pieOrder =>
                    new PieOrder
                    (
                            pieOrder.Id,
                            pieOrder.CustomerName,
                            pieOrder.CustomerAddress,
                            order.OrderLines.Select(o => o.ToDataContract()).ToList()
                    ),
                _ => throw new ArgumentException($"Unsupported goodsType type ({order.GoodsType})")
            };
        }
    }
}