using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebAPI_Train4.Models;

namespace WebAPI_Train4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangHoaController : ControllerBase
    {
        public static List<HangHoa> hangHoas = new List<HangHoa>();
        [HttpGet]
        //Interface dùng để trả về cho các action
        public IActionResult GetAll()
        {
            // Trả về danh sách các hàng hóa
            return Ok(hangHoas);
        }
        [HttpGet("{id}")]
        public IActionResult GetByID(string id)
        {
            try
            {
                //LinQ [Object] Query
                var hanghoa = hangHoas.SingleOrDefault(hh => hh.MaHangHoa == Guid.Parse(id));
                if (hanghoa == null)
                {
                    return NotFound();
                }
                return Ok(hanghoa);
            }
            catch 
            {
                return BadRequest();
            }
        }
        //Insert
        [HttpPost]
        public IActionResult Create(HangHoaVM hangHoaVM)
        {
            var hangHoa=new HangHoa 
            {
                MaHangHoa=Guid.NewGuid(),
                TenHangHoa=hangHoaVM.TenHangHoa,
                DonGia=hangHoaVM.DonGia
            };
            hangHoas.Add(hangHoa);
            return Ok(new
            {
                Success=true,Data=hangHoa
            });
        }

        [HttpPut("{id}")]
        public IActionResult Edit(string id,HangHoa hangHoaEdit)
        {
            try
            {
                //LINQ [Object] Query
                var hanghoa = hangHoas.SingleOrDefault(hh => hh.MaHangHoa == Guid.Parse(id));
                if (hanghoa==null) { return NotFound(); }
                if(id!= hanghoa.MaHangHoa.ToString()) { return BadRequest(); }
                //Update
                hanghoa.TenHangHoa=hangHoaEdit.TenHangHoa;
                hanghoa.DonGia=hangHoaEdit.DonGia;
                return Ok();
            }
            catch 
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                //Linq [object] Query
                var hanghoa = hangHoas.SingleOrDefault(hh => hh.MaHangHoa == Guid.Parse(id));
                if (hanghoa==null) { return NotFound(); }
                hangHoas.Remove(hanghoa);
                return Ok();
            }
            catch 
            {
                return BadRequest();    
            }
        }


    }
}
