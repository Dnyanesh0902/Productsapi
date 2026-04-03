using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProdApi.Data;
using ProdApi.DTOs;
using ProdApi.Models;

namespace ProdApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProduct()
        {
            var result = await _context.Products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Category = p.Category,
            }).ToListAsync();

            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(CreateProductDto dto)
        {
            var result = await _context.Products.AnyAsync(a => a.Name == dto.Name && a.Category == dto.Category);
            if (result)
                return BadRequest($"{dto.Name} and {dto.Category} Already Exist");
            var product = new Product
            {
                Name = dto.Name,
                Category = dto.Category,
                Description = dto.Description,
                Price = dto.Price,
                Quantity = dto.Quantity,
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Category = product.Category
            };
            return Ok(productDto);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await _context.Products.FindAsync(id);
            if (result == null)
                return NotFound();
            var products = new ProductDto
            {
                Category = result.Category,
                Description = result.Description,
                Price = result.Price,
                Name = result.Name,
                Id = result.Id
            };
            return Ok(products);
        }
        [HttpGet("byname/{name}")]
        public async Task<IActionResult> GetProductByName(string name)
        {
            var result = await _context.Products
                .Where(p => p.Name == name)
                .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Category = p.Category
            }).ToListAsync();
            if(!result.Any()) return NotFound();
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto dto)
        {
            var result = await _context.Products.FindAsync(id);
            if (result == null)
                return NotFound();
            result.Name = dto.Name; 
            result.Description = dto.Description;
            result.Price = dto.Price;
            result.Category = dto.Category;
            result.Quantity = dto.Quantity;
            await _context.SaveChangesAsync();

            var productDto = new ProductDto
            {
                Id = result.Id,
                Name = result.Name,
                Description = result.Description,
                Price = result.Price,
                Category = result.Category
            };
            return Ok(productDto);
        }
        [HttpPatch("{name}")]
        public async Task<IActionResult> UpdateProductByName(string name, UpdateProductDto dto)
        {
            var result = await _context.Products.FirstOrDefaultAsync(a => a.Name == name);
            if(result == null)
                return NotFound();
            result.Description= dto.Description;
            result.Price = dto.Price;
            result.Category = dto.Category;
            result.Quantity = dto.Quantity;
            await _context.SaveChangesAsync();
            var productdto = new ProductDto
            {
                Id = result.Id,
                Name = result.Name,
                Description = result.Description,
                Price = result.Price,
                Category = result.Category,
            };
            return Ok(productdto);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _context.Products.FindAsync(id);
            if (result == null)
                return NotFound();
            _context.Products.Remove(result);
            await _context.SaveChangesAsync();
            return Ok($"{result.Name} Deleted Successfully");
        }

        [HttpDelete("byname/{name}")]
        public async Task<IActionResult> DeleteProductByName(string name)
        {
            var result = await _context.Products.FirstOrDefaultAsync(n => n.Name == name);

            if(result == null)
                return NotFound();
            _context.Products.Remove(result);
            await _context.SaveChangesAsync();
            return Ok($"{result.Name} Deleted Successful.");
        }
    }
}
