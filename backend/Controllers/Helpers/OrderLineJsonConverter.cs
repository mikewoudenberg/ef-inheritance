using System;
using Model;
using Newtonsoft.Json.Linq;

namespace Controllers.Helpers
{
    public class OrderLineJsonConverter : JsonCreationConverter<OrderLine>
    {
        protected override OrderLine Create(Type objectType, JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException(nameof(jObject));

            var goodsType = jObject["goodsType"]?.ToObject<GoodsType>() ?? GoodsType.Bread;

            return goodsType switch
            {
                GoodsType.Bread => new BreadOrderLine(),
                GoodsType.Pie => new PieOrderLine(),
                _ => throw new ArgumentException($"Unsupported goodsType ({goodsType})")
            };
        }
    }
}