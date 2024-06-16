using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SellerAnalystIonic.Data;
using SellerAnalystIonic.Models;

namespace SellerAnalystIonic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/Products/{barcode}
        [HttpGet("{barcode}")]
        public async Task<ActionResult<Product>> GetProduct(string barcode)
        {
            var product = await _context.Products.FindAsync(barcode);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/{barcode}
        [HttpPut("{barcode}")]
        public async Task<IActionResult> PutProduct(string barcode, Product product)
        {
            if (barcode != product.Barcode)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(barcode))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { barcode = product.Barcode }, product);
        }

        // DELETE: api/Products/{barcode}
        [HttpDelete("{barcode}")]
        public async Task<IActionResult> DeleteProduct(string barcode)
        {
            var product = await _context.Products.FindAsync(barcode);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Products/seed
        [HttpPost("seed")]
        public async Task<IActionResult> SeedProducts([FromBody] List<Product> products)
        {
            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool ProductExists(string barcode)
        {
            return _context.Products.Any(e => e.Barcode == barcode);
        }
    }
}
