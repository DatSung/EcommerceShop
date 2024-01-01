using System;
using System.Collections.Generic;

namespace EcommerceShop.Data
{
    public partial class ChuDe
    {
        public ChuDe()
        {
            Gopies = new HashSet<GopY>();
        }

        public int MaCd { get; set; }
        public string? TenCd { get; set; }
        public string? MaNv { get; set; }

        public virtual NhanVien? MaNvNavigation { get; set; }
        public virtual ICollection<GopY> Gopies { get; set; }
    }
}
