using System;
using System.Collections.Generic;

namespace EcommerceShop.Data
{
    public partial class Loai
    {
        public Loai()
        {
            HangHoas = new HashSet<HangHoa>();
        }

        public int MaLoai { get; set; }
        public string TenLoai { get; set; } = null!;
        public string? TenLoaiAlias { get; set; }
        public string? MoTa { get; set; }
        public string? Hinh { get; set; }

        public virtual ICollection<HangHoa> HangHoas { get; set; }
    }
}
