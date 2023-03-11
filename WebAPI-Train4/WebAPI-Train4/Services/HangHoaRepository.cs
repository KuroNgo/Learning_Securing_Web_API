using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebAPI_Train4.Data;
using WebAPI_Train4.Models;

namespace WebAPI_Train4.Services
{
    public class HangHoaRepository : IHangHoaRepository
    {
        private readonly MyDbContext _context;

        // Tạo thuộc tính để người ta thích thay thì người ta thay
        // Giá trị mặc định là bằng 5
        public static int PAGE_SIZE { get; set; } = 5;
        public HangHoaRepository(MyDbContext myDbContext)
        {
            _context=myDbContext;
        }
        /// <summary>
        /// Sắp xếp:
        ///     * query.OrderBy()
        ///     * query.OrderbyDescending()
        ///     * query.ThenBy
        ///     * query.ThenByDescending
        /// Phân trạng: Skip() / Take()
        ///     * query.Skip(Skip.N).Take(pageSize)
        /// </summary>
        /// <param name="search"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="sortBy"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        // Một hàm dạng clean code thì chỉ có 3 tham số còn dạng 4 tham số là swapper
        public List<HangHoaModel> GetAll(string search,double? from, double? to,string sortBy, int page=1)
        {
            var allProducts = _context.hangHoas.Include(hh => hh.Category).AsQueryable();

            #region Filtering
            if (!string.IsNullOrEmpty(search) )
            {
                allProducts = allProducts.Where(hh => hh.TenHH.Contains(search));
            }

            // Kiểu double là kdl nguyên thủy nên có thể thêm ? để cho biết là có null hay không
            // Còn kiểu dữ liệu là object thì không cần
            if (from.HasValue)
            {
                allProducts = allProducts.Where(hh => hh.DonGia >= from);
            }
            if (to.HasValue)
            {
                allProducts = allProducts.Where(hh => hh.DonGia <= to);
            }
            #endregion

            #region Sorting
            // Default sort by Name (TenHH)
            allProducts = allProducts.OrderBy(hh => hh.TenHH);
            if(!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "tenHH_DESC":
                        allProducts = allProducts.OrderByDescending(hh => hh.TenHH); break;
                    case "gia_asc":
                        allProducts = allProducts.OrderBy(hh => hh.DonGia);break;
                    case "gia_desc":
                        allProducts = allProducts.OrderByDescending(hh => hh.DonGia); break;
                }
            }
            #endregion
            // Filter > Sorting > Paging
            // Paging là tập kết quả người ta trả về, nếu mà nhiều quá gây tắc nghẽn đường truyền
            #region Paging

            //allProducts=allProducts.Skip((page-1)*PAGE_SIZE).Take(PAGE_SIZE);
            //var result = allProducts.Select(hh => new HangHoaModel
            //{
            //    MaHangHoa = hh.MaHH,
            //    TenHangHoa = hh.TenHH,
            //    DonGia = hh.DonGia,
            //    TenLoai = hh.Category.TenLoai
            //});
            #endregion
            // Lazy loading....
            // Trả vè một cái list do Kiểu dữ liệu hàm là kiểu list
            //return result.ToList();
            var result = PaginatedList<Data.HangHoa>.Create(allProducts, page, PAGE_SIZE);
            return result.Select(hh=> new HangHoaModel 
            {
                MaHangHoa=hh.MaHH,
                TenHangHoa=hh.TenHH,
                DonGia=hh.DonGia,
                TenLoai=hh.Category?.TenLoai
            }).ToList();
        }

        public HangHoaVM GetByID(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Update(HangHoaVM categoryVM)
        {
            throw new System.NotImplementedException();
        }
    }
}
