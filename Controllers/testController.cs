using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ordermanagement.Data;
using ordermanagement.Models;

namespace ordermanagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class testController(OrdermanagementdbContext context) : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<User>> Testingsomething()
        {
            var user = context.Users.ToList();

            if (user == null)
            {
                return NotFound();
            }
            return user;
        }
    }
}
