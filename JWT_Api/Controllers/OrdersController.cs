﻿using JWT_Api.Data.Model;
using JWT_Api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JWT_Api.Data.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace JWT_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        public OrdersController(AppDbContext db)
        {
            _db = db;
        }

        private readonly AppDbContext _db;

        [HttpGet("one/{orderId:int}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _db.Orders.Where(x => x.id == orderId).FirstOrDefaultAsync();
            if (order != null)
            {
                OrderDto dto = new()
                {
                    orderId = order.id,
                    OrderDate = order.CreatedDate,
                };
                if (order.ordersItems != null && order.ordersItems.Any())
                {
                    foreach (var item in order.ordersItems)
                    {
                        OrdersItemsDto dtoItem = new()
                        {
                            itemId = item.items.Id,
                            itemName = item.items.Name,
                            price = item.Price,
                            quantity = 1,
                        };
                        dto.items.Add(dtoItem);
                    }
                }
                return Ok(dto);
            }
            return NotFound($"The Order Id {orderId} not Exists");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = _db.Orders.ToArray();
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] OrderDto order)
        {
            if (ModelState.IsValid)
            {
                Order mdl = new()
                {
                    CreatedDate = order.OrderDate,
                    ordersItems = new List<OrderItem>()
                };
                foreach (var item in order.items)
                {
                    OrderItem orderItem = new()
                    {
                        ItemId = item.itemId,
                        Price = item.price,
                    };
                    mdl.ordersItems.Add(orderItem);
                }
                await _db.Orders.AddAsync(mdl);
                await _db.SaveChangesAsync();
                order.orderId = mdl.id;
                return Ok(order);
            }
            return BadRequest();
        }
    }
}
