using System;
using System.Collections.Generic;
using Controllers.Helpers;
using Newtonsoft.Json;

namespace Model
{
    [JsonConverter(typeof(OrderJsonConverter))]
    public abstract class Order
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public GoodsType GoodsType { get; set; }
        public List<OrderLine> OrderLines { get; set; }
    }
}