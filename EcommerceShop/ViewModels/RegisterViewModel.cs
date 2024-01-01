using System.ComponentModel.DataAnnotations;

namespace EcommerceShop.ViewModels
{
    public class RegisterViewModel
    {   
        [Key]
        [Display(Name = "Username")]
        [Required(ErrorMessage = "*")]
        [MaxLength(20, ErrorMessage = "Max 20 characters")]
        public string MaKh { get; set; }



        [Display(Name = "Password")]
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }



        [Display(Name = "Fullname")]
        [Required(ErrorMessage = "*")]
        [MaxLength(50, ErrorMessage = "Max 50 characters")]
        public string HoTen { get; set; }



        [Display(Name = "Gender")]
        public bool GioiTinh { get; set; }



        [Display(Name = "Birthday")]
        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; }



        [Display(Name = "Address")]
        [MaxLength(60, ErrorMessage = "Max 60 characters")]
        public string DiaChi { get; set; }



        [Display(Name = "Phone number")]
        [MaxLength(24, ErrorMessage = "Max 24 characters")]
        [RegularExpression(@"0[987654321]\d{8}", ErrorMessage = "Invalid phone number")]
        public string DienThoai { get; set; }



        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; }



        [Display(Name = "Avatar")]
        public string? Hinh { get; set; }

    }
}
