using Microsoft.AspNetCore.Mvc;
using ordermanagement.Models;
using Newtonsoft.Json;
using ordermanagement.Dtos.User;
using ordermanagement.Mappers;
using ordermanagement.Data;
using ordermanagement.Service;
using Microsoft.EntityFrameworkCore;
using Humanizer;

namespace ordermanagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly OrdermanagementdbContext _context;

        // Inject the UserService into the controller
        public UserController(UserService userService, OrdermanagementdbContext context)
        {
            _userService = userService;
            _context = context;
        }

        [HttpPost("createOrder")]
        public IActionResult CreateOrder([FromBody] CreateOrderDto dto)
        {
            if (dto == null || !dto.Items.Any())
            {
                return BadRequest("Invalid order data.");
            }

            try
            {
                var order = _userService.CreateOrder(dto);

                _userService.SaveOrder(order);

                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating order: {ex.Message}");
            }
        }

        [HttpDelete("Order/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deleteorder = await _context.Orders.FirstOrDefaultAsync(x => x.OrderId == id);
            if (deleteorder == null)
            {
                return NotFound();
            }
            _context.Orders.Remove(deleteorder);
            

            var orderStatus = new Orderstatus
            {
                OrderId = id,
                Status = "Cancelled",
                //StatusDate = DateTime.Now
            };

            _context.Orderstatuses.Add(orderStatus);
            await _context.SaveChangesAsync();
            return Ok("order deleted");

        }

          

        }
    }

