using System;

namespace DataStores
{
    public class PieOrderLine : OrderLine
    {
        public PieOrderLine(Guid id, int sKU, string? remark) : base(id, GoodsType.Pie, sKU, remark)
        {
        }

        public PieOrderLine(Guid id, int sKU, string? remark, PiePreferences? input) : base(id, GoodsType.Pie, sKU, remark)
        {
            Input = input;
        }

        public PiePreferences? Input { get; private set; }

        public override Model.OrderLine ToModel()
        {
            return new Model.PieOrderLine()
            {
                Id = Id,
                SKU = SKU,
                Remark = Remark,
                Input = Input?.ToModel()
            };
        }
    }
}