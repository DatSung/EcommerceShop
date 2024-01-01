using System;
using System.Collections.Generic;

namespace EcommerceShop.Data
{
    public partial class TrangThai
    {
        public TrangThai()
        {
            HoaDons = new HashSet<HoaDon>();
        }

        public int MaTrangThai { get; set; }
        public string TenTrangThai { get; set; } = null!;
        public string? MoTa { get; set; }

        public virtual ICollection<HoaDon> HoaDons { get; set; }
    }
}
