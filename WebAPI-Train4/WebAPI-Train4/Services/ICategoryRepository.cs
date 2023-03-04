using System.Collections.Generic;
using WebAPI_Train4.Models;

namespace WebAPI_Train4.Services
{
    public interface ICategoryRepository
    {
        List<CategoryVM> GetAll();
        CategoryVM GetByID(int id);
        CategoryVM Add(CategoryModel categoryModel);
        void Update(CategoryVM categoryVM);
        void Remove(int id);
    }
}
