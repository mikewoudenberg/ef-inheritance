using System;

namespace DataStores
{
    public enum GoodsType
    {
        Bread,
        Pie
    }

    public static class GoodsTypeExtensions
    {
        public static Model.GoodsType ToModel(this GoodsType goodsType)
        {
            return goodsType switch
            {
                GoodsType.Bread => Model.GoodsType.Bread,
                GoodsType.Pie => Model.GoodsType.Pie,
                _ => throw new ArgumentException("Not supported")
            };
        }

        public static GoodsType ToDataContrac(this Model.GoodsType goodsType)
        {
            return goodsType switch
            {
                Model.GoodsType.Bread => GoodsType.Bread,
                Model.GoodsType.Pie => GoodsType.Pie,
                _ => throw new ArgumentException("Not supported")
            };
        }
    }
}