using EcommerceShop.Data;
using EcommerceShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceShop.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly Hshop2023Context db;

        public HangHoaController(Hshop2023Context context) 
        {
            db = context;
        }

        public IActionResult Index(int? maloai)
        {   
            var hangHoas = db.HangHoas.AsQueryable();

            if (maloai.HasValue)
            {
                hangHoas = hangHoas.Where(p => p.MaLoai == maloai.Value);
            }

            var result = hangHoas.Select(p => new HangHoaViewModel
            {
                MaHh = p.MaHh,
                TenHh = p.TenHh,
                DonGia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            }); 

            return View(result);
        }

        public IActionResult Search(string query)
        {

            var hangHoas = db.HangHoas.AsQueryable();

            if (query != null)
            {
                hangHoas = hangHoas.Where(p => p.TenHh.Contains(query));
            }

            var result = hangHoas.Select(p => new HangHoaViewModel
            {
                MaHh = p.MaHh,
                TenHh = p.TenHh,
                DonGia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            });


            return View(result);
        }

        public IActionResult Detail(int mahh) 
        {
            var data = db.HangHoas.Include(p => p.MaLoaiNavigation).SingleOrDefault(p => p.MaHh == mahh);
            if (data == null)
            {
                TempData["Message"] = $"Not found this item with id {mahh}";
                return Redirect("/404");
            }

            var result = new DetailHangHoaViewModel 
            { 
                MaHh = data.MaHh,
                TenHh = data.TenHh,
                DonGia = data.DonGia ?? 0,
                ChiTiet = data.MoTa ?? string.Empty,
                Hinh = data.Hinh ?? string.Empty,
                MoTaNgan = data.MoTaDonVi ?? string.Empty,
                TenLoai = data.MaLoaiNavigation.TenLoai,
                SoLuongTon = 10, //Coming soon
                DiemDanhGia = 5, //Comming soon
            };

            return View(result);
        }
    }
}
