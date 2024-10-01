using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using RestaurantFaves.Models;
namespace RestaurantFaves.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        RestaurantContext dbContext = new RestaurantContext();

        [HttpGet()]
        public IActionResult GetAll(string? restaurant = null,Boolean? orderagain = null)
        {
            List<Order> result = dbContext.Orders.ToList();
            if (restaurant != null)
            {
                result = result.Where(p => p.Restaurant.ToLower().Contains(restaurant.ToLower())).ToList();

            }
            if (orderagain != null)
            {
                result = result.Where(p => p.OrderAgain == orderagain).ToList();

            }
            return Ok(result);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Order result = dbContext.Orders.FirstOrDefault(p => p.Id == id);
            if (result == null)
            {
                return NotFound("No matching Id");
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpPost()]
        public IActionResult AddOrder([FromBody] Order newOrder)
        {
            newOrder.Id = 0;
            dbContext.Orders.Add(newOrder);
            dbContext.SaveChanges();
            return Created($"/api/Posts/{newOrder.Id}", newOrder);
        }
        [HttpPut("{id}")]
        public IActionResult UpdatePost(int id, [FromBody] Order updated)
        {
            if (updated.Id != id) { return BadRequest("Ids don't match"); }
            if (dbContext.Orders.Any(p => p.Id == id) == false) { return NotFound("No matching ids"); }
            dbContext.Orders.Update(updated);
            dbContext.SaveChanges();
            return Ok(updated);
        }
        [HttpDelete("{id}")]
        public IActionResult DeletePost(int id)
        {
            Order result = dbContext.Orders.FirstOrDefault(p => p.Id == id);
            if (result == null)
            {
                return NotFound("No matching id");
            }
            else
            {
                dbContext.Orders.Remove(result);
                dbContext.SaveChanges();
                return NoContent();
            }
        }


    }
}
