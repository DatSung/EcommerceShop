using System.ComponentModel.DataAnnotations;

namespace EcommerceShop.ViewModels
{
    public class LoginViewModel
    {

        [Display(Name = "UserName")]
        [Required(ErrorMessage = "You must enter the UserName")]
        [MaxLength(20, ErrorMessage = "Max 20 characters")]
        public string UserName { get; set; }


        [Display(Name = "Password")]
        [Required(ErrorMessage = "You must enter the Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
