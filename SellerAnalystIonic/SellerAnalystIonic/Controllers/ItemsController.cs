using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SellerAnalystIonic.Models;
using SellerAnalystIonic.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SellerAnalystIonic.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly DataContext _context;

        public ItemsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _context.Items.ToListAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpGet("object/{objectName}")]
        public async Task<IActionResult> GetByObject(string objectName)
        {
            var items = await _context.Items.Where(i => i.Object == objectName).ToListAsync();
            if (!items.Any())
                return NotFound();
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Item newItem)
        {
            _context.Items.Add(newItem);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newItem.ID }, newItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Item updatedItem)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
                return NotFound();

            item.Object = updatedItem.Object;
            item.Price = updatedItem.Price;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
                return NotFound();

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
