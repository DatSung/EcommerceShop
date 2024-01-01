using System;
using System.Collections.Generic;

namespace EcommerceShop.Data
{
    public partial class NhanVien
    {
        public NhanVien()
        {
            ChuDes = new HashSet<ChuDe>();
            HoaDons = new HashSet<HoaDon>();
            HoiDaps = new HashSet<HoiDap>();
            PhanCongs = new HashSet<PhanCong>();
        }

        public string MaNv { get; set; } = null!;
        public string HoTen { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? MatKhau { get; set; }

        public virtual ICollection<ChuDe> ChuDes { get; set; }
        public virtual ICollection<HoaDon> HoaDons { get; set; }
        public virtual ICollection<HoiDap> HoiDaps { get; set; }
        public virtual ICollection<PhanCong> PhanCongs { get; set; }
    }
}
