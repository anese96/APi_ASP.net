using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Test_API.Data;
using Test_API.Data.Models;
using Test_API.Models;

namespace Test_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        public ItemsController(AppDbContext db)
        {
            _db = db;
        }
        private readonly AppDbContext _db;

        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var items = await _db.Items.ToListAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            var item = await _db.Items.SingleOrDefaultAsync(x => x.IdItem ==id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpGet("Category/{idCategory}")]
        public async Task<IActionResult> GetItemByIdCategory(int idCategory)
        {
            var item =  _db.Items.Where(x => x.CategoryId == idCategory).ToList();
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem( [FromForm] mdlitem mdl)
        {
            using var stream = new MemoryStream();
            await mdl.Image.CopyToAsync(stream);
          var item = new Item
          {
              Name = mdl.Name,
              Price = mdl.Price,
              Notes = mdl.Notes,
              Image = stream.ToArray(),
              CategoryId = mdl.CategoryId
          };
            await _db.Items.AddAsync(item);
            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromForm] mdlitem mdl )
        {
          var item = await _db.Items.FindAsync(id);
            if (item == null) { 
             return NotFound();
            }
            var isCategory= await _db.Categoryes.AnyAsync(x=>x.CategoryId==mdl.CategoryId);
            if (isCategory)
            {
                return NotFound();
            }
            if (mdl.Image != null)
            {
                using var stream = new MemoryStream();
                await mdl.Image.CopyToAsync(stream);
                item.Image = stream.ToArray();
            }
            item.Name = mdl.Name;
            item.Price = mdl.Price;
            item.Notes = mdl.Notes;
            item.CategoryId = mdl.CategoryId;
            await _db.SaveChangesAsync();
            return Ok();


        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemById(int id)
        {
            var item = await _db.Items.SingleOrDefaultAsync(x => x.IdItem == id);
            if (item == null)
            {
                return NotFound();
            }
            _db.Items.Remove(item);
            await _db.SaveChangesAsync();

            return Ok(item);
        }
    }
}
