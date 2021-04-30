using System;
using Model;
using Newtonsoft.Json.Linq;

namespace Controllers.Helpers
{
    public class OrderJsonConverter : JsonCreationConverter<Order>
    {
        protected override Order Create(Type objectType, JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException(nameof(jObject));

            var goodsType = jObject["goodsType"]?.ToObject<GoodsType>() ?? GoodsType.Bread;

            return goodsType switch
            {
                GoodsType.Bread => new BreadOrder(),
                GoodsType.Pie => new PieOrder(),
                _ => throw new ArgumentException($"Unsupported goodsType ({goodsType})")
            };
        }
    }
}