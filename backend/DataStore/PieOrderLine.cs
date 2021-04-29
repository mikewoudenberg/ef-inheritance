using System;

namespace DataStores
{
    public class PieOrderLine : OrderLine
    {
        public PieOrderLine(Guid id, int sKU, string? remark) : base(id, GoodsType.Pie, sKU, remark)
        {
        }

        public PieOrderLine(Guid id, int sKU, string? remark, PiePreferences? piePreferences) : base(id, GoodsType.Pie, sKU, remark)
        {
            PiePreference = piePreferences;
        }

        public PiePreferences? PiePreference { get; private set; }
    }
}