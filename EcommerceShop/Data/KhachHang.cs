using System;
using System.Collections.Generic;

namespace EcommerceShop.Data
{
    public partial class KhachHang
    {
        public KhachHang()
        {
            BanBes = new HashSet<BanBe>();
            HoaDons = new HashSet<HoaDon>();
            YeuThiches = new HashSet<YeuThich>();
        }

        public string MaKh { get; set; } = null!;
        public string? MatKhau { get; set; }
        public string HoTen { get; set; } = null!;
        public bool GioiTinh { get; set; }
        public DateTime NgaySinh { get; set; }
        public string? DiaChi { get; set; }
        public string? DienThoai { get; set; }
        public string Email { get; set; } = null!;
        public string? Hinh { get; set; }
        public bool HieuLuc { get; set; }
        public int VaiTro { get; set; }
        public string? RandomKey { get; set; }

        public virtual ICollection<BanBe> BanBes { get; set; }
        public virtual ICollection<HoaDon> HoaDons { get; set; }
        public virtual ICollection<YeuThich> YeuThiches { get; set; }
    }
}
