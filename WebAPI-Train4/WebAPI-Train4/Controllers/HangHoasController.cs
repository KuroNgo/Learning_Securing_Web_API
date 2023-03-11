using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Train4.Services;

namespace WebAPI_Train4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangHoasController : ControllerBase
    {
        private readonly HangHoaRepository _hangHoaRepository;

        public HangHoasController(HangHoaRepository hangHoaRepository) 
        {
            _hangHoaRepository=hangHoaRepository;
        }

        [HttpGet]
        public IActionResult GetAllProducts(string search,double? from, double? to,string sortBy, int page = 1)
        {
            try
            {
                var result = _hangHoaRepository.GetAll(search,from,to,sortBy,page);
                return Ok(result);
            }
            catch (System.Exception)
            {
                return BadRequest("We can not get the product");
            }
        }
    }
}
