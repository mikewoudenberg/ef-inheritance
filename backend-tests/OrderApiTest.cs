using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using DataStores;
using FluentAssertions;
using Helpers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Xunit;

namespace backend_tests
{
    public class OrderApiTest
    {
        [Fact]
        public async Task UpdateOrderAddProducts_StoresNew()
        {
            //Arrange
            var (client, dbContext) = IntegrationTestBootstrapper.Build();
            var createdResponse = await client.PostAsync("/order", new StringContent("", Encoding.UTF8, MediaTypeNames.Application.Json));
            string json = await createdResponse.Content.ReadAsStringAsync();
            var adjustedOrder = JObject.Parse(json);
            var newOrderLine = JObject.Parse(@"{
                'id': 'd21b08e4-16f4-4ae9-83db-b1fc7a10c4df',
                'goodsType': 0,
                'sKU': 14713499,
                'remark': 'bring it home',
                'input': {
                    'wheat': true,
                    'seeds': false
                }
            }");
            var orderLines = (JArray)adjustedOrder["orderLines"];
            orderLines.Add(newOrderLine);

            //Act
            var result = await client.PutAsync($"/order/{(string)adjustedOrder["id"]}", new StringContent(adjustedOrder.ToString(), Encoding.UTF8, MediaTypeNames.Application.Json));

            //Assert
            result.EnsureSuccessStatusCode();
            var returnedOrder = JObject.Parse(await result.Content.ReadAsStringAsync());
            returnedOrder["orderLines"].Last()["input"].Should().NotBeNull();
            var order = dbContext.Orders.Include(o => o.OrderLines).Single();
            order.OrderLines.Should().HaveCount(orderLines.Count);
            order.OrderLines.Last().As<BreadOrderLine>().Input.Should().NotBeNull();
        }

        [Fact]
        public async Task GetOrder_ReturnsStored()
        {
            //Arrange
            var (client, dbContext) = IntegrationTestBootstrapper.Build();
            var order = new BreadOrder(Guid.NewGuid(), "someone", "here",
                new() {new BreadOrderLine(Guid.NewGuid(), 12345, "Must be good", new BreadPreferences(true, false))});
            dbContext.Orders.Add(order);
            dbContext.SaveChanges();

            //Act
            var result = await client.GetAsync($"/order/{order.Id}");

            //Assert
            result.EnsureSuccessStatusCode();
            var returnedOrder = JObject.Parse(await result.Content.ReadAsStringAsync());
            returnedOrder["orderLines"].Last()["input"].Should().NotBeNull();
        }

        [Fact]
        public async Task Insert()
        {
            var databaseName = Guid.NewGuid().ToString();
            using (var context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName).Options))
            {
                await context.Orders.AddAsync(new BreadOrder(Guid.NewGuid(), "Mike", "here", new List<OrderLine>
                {
                    new BreadOrderLine(new Guid("d21b08e4-16f4-4ae9-83db-b1fc7a10c4df"), 14713499, "bring it home",
                        new BreadPreferences(true, false))
                }));

                await context.SaveChangesAsync();
            }
            
            using (var context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName).Options))
            {
                var order = context.Orders.Include(o => o.OrderLines).Single();
                order.OrderLines.Should().HaveCount(1);
                order.OrderLines.Last().As<BreadOrderLine>().Input.Should().NotBeNull();
            }
        }
        
        [Fact]
        public async Task Update()
        {
            var databaseName = Guid.NewGuid().ToString();
            var id = Guid.NewGuid();

            await using (var context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName).Options))
            {
                await context.Orders.AddAsync(new BreadOrder(id, "Mike", "here"));
                await context.SaveChangesAsync();
            }

            await using (var context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName).Options))
            {
                var order = await context.Orders.FindAsync(id);
                var line = new BreadOrderLine(new Guid("d21b08e4-16f4-4ae9-83db-b1fc7a10c4df"), 14713499, "bring it home", new BreadPreferences(true, false));
                order.OrderLines.Add(line);

                context.Attach(line).State = EntityState.Added;

                await context.SaveChangesAsync();
            }

            await using (var context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName).Options))
            {
                var order = context.Orders.Include(o => o.OrderLines).Single();
                order.OrderLines.Should().HaveCount(1);
                order.OrderLines.Last().As<BreadOrderLine>().Input.Should().NotBeNull();
            }
        }
    }
}
