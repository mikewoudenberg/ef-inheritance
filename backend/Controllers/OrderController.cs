using System;
using System.Threading.Tasks;
using Model;
using Microsoft.AspNetCore.Mvc;

namespace ef_issue.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly DataStores.OrderRepository _orderRepository;

        public OrderController(DataStores.OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }


        [HttpPost]
        public async Task<ActionResult<Order>> Create()
        {
            var result = await _orderRepository.CreateOrder(new BreadOrder { Id = Guid.NewGuid(), CustomerName = "Mike", CustomerAddress = "Here", OrderLines = new() });
            return Ok(result);
        }

        [HttpPut("{orderId}")]
        public async Task<ActionResult<Order>> Update(string orderId, Order order)
        {
            var result = await _orderRepository.UpdateOrder(order);

            return Ok(result);
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<Order>> Read(string orderId)
        {
            var result = await _orderRepository.GetOrderById(Guid.Parse(orderId));

            return Ok(result);
        }

    }
}
