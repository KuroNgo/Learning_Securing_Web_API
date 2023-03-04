using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Train4.Models;
using WebAPI_Train4.Services;

namespace WebAPI_Train4.Controllers
{
    // 5 API cơ bản
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_categoryRepository.GetAll());
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetByID(int id)
        {
            try
            {
                var data = _categoryRepository.GetByID(id);
                if (data != null)
                {
                    return Ok(data);
                }
                else
                {
                    return NotFound();
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id,CategoryVM categoryVM)
        {
            if(id != categoryVM.MaLoai)
            {
                return BadRequest();
            }
            try
            {
                _categoryRepository.Update(categoryVM);
                return NoContent();//204
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _categoryRepository.Remove(id);
                return Ok();
            }
            catch 
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult Add(CategoryModel categoryModel)
        {
            try
            {
                return Ok(_categoryRepository.Add(categoryModel));
            }
            catch 
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
