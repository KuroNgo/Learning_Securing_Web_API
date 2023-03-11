using System.Collections.Generic;
using WebAPI_Train4.Models;

namespace WebAPI_Train4.Services
{
    public interface IHangHoaRepository
    {
        // TÌm kiếm
        List<HangHoaModel> GetAll(string search, double? from, double? to,string sortBy,int page =1);
        HangHoaVM GetByID(int id);
        void Update(HangHoaVM categoryVM);
        void Remove(int id);
    }
}
