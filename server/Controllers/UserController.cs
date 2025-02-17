using Microsoft.AspNetCore.Mvc;
using ordermanagement.Models;
using ordermanagement.Dtos.User;
using ordermanagement.Service;
using ordermanagement.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ordermanagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IOrderCreatePublisher _orderCreatePublisher;

        public UserController(UserService userService,IOrderCreatePublisher orderCreatePublisher)
        {
            _userService = userService;
            _orderCreatePublisher = orderCreatePublisher;
        }

        [HttpPost("createOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            if (dto == null || !dto.Items.Any())
            {
                return BadRequest("Invalid order data.");
            }

            try
            {
                var order = await _userService.CreateOrder(dto);
            
                await _orderCreatePublisher.PublishOrderCreateUpdate(order);
                
                Console.WriteLine("Order status updated and event published");
                return Ok(order);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpDelete("order/{id}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] int id)
        {
            var result = await _userService.DeleteOrder(id);
            return Ok(result);
        }

         [HttpGet("orders/{id}")]
        public async Task<IEnumerable<OrderDto>> GetOrder([FromRoute] int id)
        {
            var result = await _userService.GetOrdersByUid(id);
            return result;
        }
    }
}
