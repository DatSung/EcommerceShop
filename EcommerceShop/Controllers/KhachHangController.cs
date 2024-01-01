using AutoMapper;
using EcommerceShop.Data;
using EcommerceShop.Helpers;
using EcommerceShop.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace EcommerceShop.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly Hshop2023Context db;
        private readonly IMapper _mapper;

        public KhachHangController(Hshop2023Context context, IMapper mapper) 
        {
            db = context;
            _mapper = mapper;
        }


        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("Profile");
            }
            return View();
        }


        #region Register
        [HttpPost]
        public IActionResult Register(RegisterViewModel model, IFormFile Hinh)
        {   
            if (ModelState.IsValid)
            {
                try
                {
                    var khachhang = _mapper.Map<KhachHang>(model);
                    khachhang.RandomKey = MyUtil.GenerateRandomKey();
                    khachhang.MatKhau = model.MatKhau.ToMd5Hash(khachhang.RandomKey);
                    khachhang.HieuLuc = true; //Coming soon when using email to active
                    khachhang.VaiTro = 0;

                    if (Hinh != null)
                    {
                        khachhang.Hinh = MyUtil.UpLoadImg(Hinh, "KhachHang");
                    }

                    db.Add(khachhang);
                    db.SaveChanges();

                    return RedirectToAction("Index", "HangHoa");
                }
                catch (Exception ex)
                {

                }
            }

            return View();
        }
        #endregion



        #region Login
        [HttpGet]
        public IActionResult Login(string? ReturnUrl)
        {   
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("Profile");
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (ModelState.IsValid)
            {
                var khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == model.UserName);

                if (khachHang == null)
                {
                    ModelState.AddModelError("Error", "Accout does not exist");
                }
                else
                {
                    if(!khachHang.HieuLuc)
                    {
                        ModelState.AddModelError("Error", "Your account has been locked");
                    }
                    else
                    {
                        if (khachHang.MatKhau != model.Password.ToMd5Hash(khachHang.RandomKey))
                        {
                            ModelState.AddModelError("Error", "Your information is incorrect");
                        }
                        else
                        {
                            var claims = new List<Claim> {
                                new Claim(ClaimTypes.Email, khachHang.Email),
                                new Claim(ClaimTypes.Name, khachHang.HoTen),
                                new Claim(MyConst.CLAIM_CUSTOMERID, khachHang.MaKh),

                                //claim - role dynamic

                                new Claim(ClaimTypes.Role, "Customer")
                            };

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                            await HttpContext.SignInAsync(claimsPrincipal);

                            if (Url.IsLocalUrl(ReturnUrl))
                            {
                                return Redirect(ReturnUrl);
                            } else 
                            { 
                                return Redirect("/"); 
                            }
                        }
                    }
                } 
            }
            return View();
        }

        #endregion

        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
