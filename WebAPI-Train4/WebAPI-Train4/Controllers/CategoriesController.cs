using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebAPI_Train4.Data;
using WebAPI_Train4.Models;

namespace WebAPI_Train4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private MyDbContext _context;

        public CategoriesController(MyDbContext myDbContext)
        {
            _context = myDbContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var dsLoai = _context.categories.ToList();
                return Ok(dsLoai);
            }
            catch
            {
                return BadRequest();
            }
            
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
        [Authorize] 
        //401
        // Phải cấu hình mới thực hiện được lệnh authorized
        // Phải đăng nhập mới được làm
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
                //return Ok(loai)
                return StatusCode(StatusCodes.Status201Created, loai);
            }
            catch
            {
                //400: BadRequest
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
                //404
                return NotFound();
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteByID(int id)
        {
            var dsLoai = _context.categories.SingleOrDefault(loai =>
            loai.MaLoai == id);
            if (dsLoai != null)
            {
                _context.Remove(dsLoai);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status200OK);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
