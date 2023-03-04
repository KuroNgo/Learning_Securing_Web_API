using System;
using System.Collections.Generic;
using System.Linq;
using WebAPI_Train4.Data;
using WebAPI_Train4.Models;

namespace WebAPI_Train4.Services
{
    public class LoaiRepository : ICategoryRepository
    {
        private readonly MyDbContext _context;
        public LoaiRepository(MyDbContext myDbContext)
        {
            _context = myDbContext;
        }
        public CategoryVM Add(CategoryModel categoryModel)
        {
            var loai = new Category
            {
                TenLoai = categoryModel.TenLoai,
            };
            _context.Add(loai);
            _context.SaveChanges();
            return new CategoryVM
            {
                MaLoai = loai.MaLoai,
                TenLoai = loai.TenLoai
            };
        }

        public List<CategoryVM> GetAll()
        {
            var loais = _context.categories.Select(loai => new CategoryVM
            {
                MaLoai = loai.MaLoai,
                TenLoai = loai.TenLoai,
            });
            return loais.ToList();
        }

        public CategoryVM GetByID(int id)
        {
            var loais = _context.categories.SingleOrDefault(loai => loai.MaLoai == id);
            if(loais != null)
            {
                return new CategoryVM
                {
                    MaLoai = loais.MaLoai,
                    TenLoai = loais.TenLoai,
                };
            }
            return null;
        }

        public void Remove(int id)
        {
            var loai = _context.categories.SingleOrDefault(loai => loai.MaLoai == id);
            if(loai != null)
            {
                _context.Remove(loai);
                _context.SaveChanges();
            }
        }

        public void Update(CategoryVM categoryVM)
        {
            var loai = _context.categories.SingleOrDefault(loai => loai.MaLoai == categoryVM.MaLoai);
            loai.TenLoai = categoryVM.TenLoai;
            _context.SaveChanges();
        }
    }
}
