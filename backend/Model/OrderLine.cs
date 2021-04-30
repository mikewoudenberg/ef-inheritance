using System;
using Controllers.Helpers;
using Newtonsoft.Json;

namespace Model
{
    [JsonConverter(typeof(OrderLineJsonConverter))]
    public abstract class OrderLine
    {
        public Guid Id { get; set; }
        public GoodsType GoodsType { get; set; }
        public int SKU { get; set; }
        public string? Remark { get; set; }
    }
}