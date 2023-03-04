using System.Collections.Generic;
using System.Linq;
using WebAPI_Train4.Models;

namespace WebAPI_Train4.Services
{
    public class CategoryRepositoryInMemory : ICategoryRepository
    {
        // Tạo mảng tỉnh
        static List<CategoryVM> loais = new List<CategoryVM>
        {
            new CategoryVM{MaLoai = 1, TenLoai= "Tivi"},
            new CategoryVM{MaLoai = 2, TenLoai= "Tủ lạnh"},
            new CategoryVM{MaLoai = 3, TenLoai= "Điều hòa"},
            new CategoryVM{MaLoai = 4, TenLoai= "Máy giặt"}
        };
        public CategoryVM Add(CategoryModel categoryModel)
        {
            var loai= new CategoryVM
            {
                MaLoai = loais.Max(loai => loai.MaLoai)+1,
                TenLoai = categoryModel.TenLoai,
            };
            loais.Add(loai);
            return loai;
        }

        public List<CategoryVM> GetAll()
        {
            return loais;
        }

        public CategoryVM GetByID(int id)
        {
            return loais.SingleOrDefault(loai => loai.MaLoai == id);
        }

        public void Remove(int id)
        {
            var _loai = loais.SingleOrDefault(loai => loai.MaLoai == id);
            loais.Remove(_loai);
        }

        public void Update(CategoryVM categoryVM)
        {
            var _loai = loais.SingleOrDefault(loai => loai.MaLoai == loai.MaLoai);
            if (_loai != null)
            {
                _loai.TenLoai = categoryVM.TenLoai;
            }
        }
    }
}
