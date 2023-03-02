using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebAPI_Train4.Data;
using WebAPI_Train4.Models;

namespace WebAPI_Train4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private MyDbContext _context;

        public CategoryController(MyDbContext myDbContext)
        {
            _context = myDbContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var dsLoai = _context.categories.ToList();
            return Ok(dsLoai);
        }

        [HttpGet("{id}")]
        public IActionResult GetByID(int id)
        {
            var dsLoai = _context.categories.SingleOrDefault(loai =>
            loai.MaLoai == id);
            if (dsLoai != null)
            {
                return Ok(dsLoai);
            }
            else
            {
                return NotFound();
            }
        }

        // Thêm 
        [HttpPost]
        public IActionResult CreateNew(CategoryModel categoryModel)
        {
            try
            {
                var loai = new Category
                {
                    TenLoai = categoryModel.TenLoai
                };
                _context.Add(loai);
                _context.SaveChanges();
                return Ok(loai);
            }
            catch
            {
                return BadRequest();
            }

        }

        //
        [HttpPut("{id}")]
        public IActionResult UpdateCategoryById(int id,CategoryModel categoryModel)
        {
            var category = _context.categories.SingleOrDefault(category => category.MaLoai == id);
            if(category != null)
            {
                category.TenLoai=categoryModel.TenLoai;
                _context.SaveChanges();
                return NoContent();

            }
            else
            {
                return NotFound();
            }
        }
    }
}
