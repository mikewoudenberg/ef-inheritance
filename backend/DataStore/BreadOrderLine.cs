using System;

namespace DataStores
{
    public class BreadOrderLine : OrderLine
    {
        public BreadOrderLine(Guid id, int sKU, string? remark) : base(id, GoodsType.Bread, sKU, remark)
        {
        }

        public BreadOrderLine(Guid id, int sKU, string? remark, BreadPreferences? breadPreferences) : base(id, GoodsType.Bread, sKU, remark)
        {
            BreadPreferences = breadPreferences;
        }

        public BreadPreferences? BreadPreferences { get; private set; }
    }
}