using System;

namespace DataStores
{
    public abstract class OrderLine
    {
        protected OrderLine(Guid id, GoodsType goodsType, int sKU, string? remark)
        {
            Id = id;
            GoodsType = goodsType;
            SKU = sKU;
            Remark = remark;
        }

        public Guid Id { get; private set; }
        public GoodsType GoodsType { get; private set; }
        public int SKU { get; private set; }
        public string? Remark { get; private set; }
        public abstract Model.OrderLine ToModel();
    }

    public static class OrderLineExtensions
    {
        public static OrderLine ToDataContract(this Model.OrderLine orderLine)
        {
            return orderLine switch
            {
                Model.BreadOrderLine breadOrderLine => new BreadOrderLine(
                    breadOrderLine.Id,
                    breadOrderLine.SKU,
                    breadOrderLine.Remark,
                    breadOrderLine.Input?.ToDataContract()
                ),
                Model.PieOrderLine pieOrderLine => new PieOrderLine(
                    pieOrderLine.Id,
                    pieOrderLine.SKU,
                    pieOrderLine.Remark,
                    pieOrderLine.Input?.ToDataContract()
                ),
                _ => throw new ArgumentException($"Unsupported goodsType ({orderLine.GoodsType})")
            };
        }
    }
}