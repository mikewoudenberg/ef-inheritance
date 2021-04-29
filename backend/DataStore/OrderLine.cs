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
    }
}