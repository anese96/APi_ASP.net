using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Test_API.Data;
using Test_API.Data.Models;

namespace Test_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        public CategoriesController(AppDbContext db)
        {
            _db = db;

        }
        private readonly AppDbContext _db;

        [HttpGet]
        public async Task<IActionResult> GetCategoryes()
        {
            var categoryes = await _db.Categoryes.ToListAsync();           

            return Ok(categoryes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryeById(int id)
        {
            
            var categorye = await _db.Categoryes.SingleOrDefaultAsync(x => x.CategoryId == id);
            if (categorye == null)
            {
                return NotFound();
            }
            return Ok(categorye);
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(string category)
        {
            Category c = new () { Name = category };
            await _db.Categoryes.AddAsync(c);
            _db.SaveChanges();
            return Ok();
        }
        [HttpPut]

        public async Task<IActionResult> UpdateCategory(Category category)
        {
            var c = await _db.Categoryes.SingleOrDefaultAsync(x => x.CategoryId == category.CategoryId);
            if (c == null)
            {
                return NotFound();
            }
            c.Name = category.Name;
            _db.SaveChanges();
            return Ok(c);
        }
        [HttpPatch("{id}")]

        public async Task<IActionResult> UpdateCategoryPatch( [FromBody] JsonPatchDocument <Category> category, [FromRoute] int id)
        {
            var c = await _db.Categoryes.SingleOrDefaultAsync(x => x.CategoryId == id);
            if (c == null)
            {
                return NotFound();
            }
           category.ApplyTo(c);
           await _db.SaveChangesAsync();
            return Ok(c);
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteCategory(int id)
        {
            var c = await _db.Categoryes.SingleOrDefaultAsync(x => x.CategoryId == id);
            if (c == null)
            {
                return NotFound();
            }
            _db.Categoryes.Remove(c);
            _db.SaveChanges();
            return Ok();
        }
    }
}
