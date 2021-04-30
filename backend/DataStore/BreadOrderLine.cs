using System;

namespace DataStores
{
    public class BreadOrderLine : OrderLine
    {
        public BreadOrderLine(Guid id, int sKU, string? remark) : base(id, GoodsType.Bread, sKU, remark)
        {
        }

        public BreadOrderLine(Guid id, int sKU, string? remark, BreadPreferences? input) : base(id, GoodsType.Bread, sKU, remark)
        {
            Input = input;
        }

        public BreadPreferences? Input { get; private set; }

        public override Model.OrderLine ToModel()
        {
            return new Model.BreadOrderLine()
            {
                Id = Id,
                SKU = SKU,
                Remark = Remark,
                Input = Input?.ToModel()
            };
        }
    }
}