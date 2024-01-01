using EcommerceShop.Data;
using EcommerceShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceShop.ViewComponents
{
    public class MenuTypeViewComponent : ViewComponent
    {
        private readonly Hshop2023Context db;
        public MenuTypeViewComponent(Hshop2023Context context) => db = context;
        public IViewComponentResult Invoke()
        {
            var data = db.Loais.Select(lo => new MenuTypeViewModel
            {
                MaLoai = lo.MaLoai,
                TenLoai = lo.TenLoai,
                SoLuong = lo.HangHoas.Count
            }).OrderBy(p => p.TenLoai);
            return View(data); 
        }

    }
}
