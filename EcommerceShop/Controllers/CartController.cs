using EcommerceShop.Data;
using EcommerceShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using EcommerceShop.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace EcommerceShop.Controllers
{
    public class CartController : Controller
    {
        private readonly Hshop2023Context db;
        private readonly PaypalClient _paypalClient;

        public CartController(Hshop2023Context context, PaypalClient paypalClient) 
        {
            db = context;
            _paypalClient = paypalClient;
        }


        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(MyConst.CART_KEY) ?? new List<CartItem>();

        public IActionResult Index()
        {
            return View(Cart);
        }

        public IActionResult AddToCart(int id, int quantity = 1, string btnquantity = "")
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == id);

            if (item == null) 
            {
                var hangHoa = db.HangHoas.SingleOrDefault(p => p.MaHh == id);
                if (hangHoa == null)
                {
                    TempData["Message"]= $"Product not found with id = {id}";
                    return Redirect("/404");
                }
                else
                {
                    item = new CartItem
                    {
                        MaHh = hangHoa.MaHh,
                        Hinh = hangHoa.Hinh ?? string.Empty,
                        TenHh = hangHoa.TenHh,
                        DonGia = hangHoa.DonGia ?? 0,
                        SoLuong = quantity,
                    };
                    gioHang.Add(item);
                }

            }
            else
            {   
                if (btnquantity == "increase" || btnquantity == "")
                {
                    item.SoLuong += quantity;
                }

                if (btnquantity == "decrease")
                {
                    if (item.SoLuong > 1)
                    {
                        item.SoLuong -= quantity;
                    }
                }
                
            }

            HttpContext.Session.Set(MyConst.CART_KEY, gioHang);

            return RedirectToAction("Index");
        }

        public IActionResult RemoveItem(int id)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == id);

            if (item != null) 
            {
                gioHang.Remove(item);
                HttpContext.Session.Set(MyConst.CART_KEY, gioHang);
            }

            return RedirectToAction("Index");

        }

        [Authorize]
        [HttpGet]
        public IActionResult Checkout() 
        {

            if (Cart.Count == 0)
            {
                return Redirect("/HangHoa");
            }

            ViewBag.PaypalClientId = _paypalClient.ClientId;
            return View(Cart);
        }


        [Authorize]
        [HttpPost]
        public IActionResult Checkout(CheckoutViewModel model)
        {

            if (ModelState.IsValid)
            {
                var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MyConst.CLAIM_CUSTOMERID).Value;

                var khachHang = new KhachHang();

                if(model.CusInfo)
                {
                    khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == customerId);
                }

                var hoadon = new HoaDon
                {
                    MaKh = customerId,
                    HoTen = model.HoTen ?? khachHang.HoTen,
                    DiaChi = model.DiaChi ?? khachHang.DiaChi,
                    DienThoai = model.DienThoai ?? khachHang.DienThoai,
                    NgayDat = DateTime.Now,
                    CachThanhToan = "COD",
                    CachVanChuyen = "Grab",
                    MaTrangThai = 0,
                    GhiChu = model.GhiChu 
                };


                db.Database.BeginTransaction();
                try
                {
                    db.Database.CommitTransaction();
                    db.Add(hoadon);
                    db.SaveChanges();

                    var cthds = new List<ChiTietHd>();

                    foreach (var item in Cart)
                    {
                        cthds.Add(new ChiTietHd
                        {
                            MaHd = hoadon.MaHd,
                            SoLuong = item.SoLuong,
                            DonGia = item.MaHh,
                            MaHh = item.MaHh,   
                            GiamGia = 0
                        });
                    }

                    db.AddRange(cthds);

                    db.SaveChanges();

                    HttpContext.Session.Set<List<CartItem>>(MyConst.CART_KEY, new List<CartItem>());

                    return View("Success");
                }
                catch
                {
                    db.Database.RollbackTransaction();
                }

            }

            return View(Cart);
        }


        #region Paypal Payment
        [Authorize]
        [HttpPost("/Cart/create-paypal-order")]
        public async Task<IActionResult> CreatePaypalOrder(CancellationToken cancellationToken)
        {
            //Thong tin don hang gui qua Paypal
            var tongtien = Cart.Sum(p => p.ThanhTien).ToString();
            var donViTienTe = "USD";
            var maDonHangThamChieu = "DH" + DateTime.Now.Ticks.ToString(); ;


            try
            {
                var response = await _paypalClient.CreateOrder(tongtien, donViTienTe, maDonHangThamChieu);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = new {ex.GetBaseException().Message};
                return BadRequest(error);
            }

        }


        [Authorize]
        [HttpPost("/Cart/capture-paypal-order")]
        public async Task<IActionResult> CapturePaypalOrder(string orderID, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _paypalClient.CaptureOrder(orderID);

                var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MyConst.CLAIM_CUSTOMERID).Value;

                var khachHang = new KhachHang();

                khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == customerId);

                var hoadon = new HoaDon
                {
                    MaKh = customerId,
                    HoTen = khachHang.HoTen,
                    DiaChi = khachHang.DiaChi,
                    DienThoai = khachHang.DienThoai,
                    NgayDat = DateTime.Now,
                    CachThanhToan = "Paypal",
                    CachVanChuyen = "Grab",
                    MaTrangThai = 0,
                    GhiChu = ""
                };


                db.Database.BeginTransaction();
                try
                {
                    db.Database.CommitTransaction();
                    db.Add(hoadon);
                    db.SaveChanges();

                    var cthds = new List<ChiTietHd>();

                    foreach (var item in Cart)
                    {
                        cthds.Add(new ChiTietHd
                        {
                            MaHd = hoadon.MaHd,
                            SoLuong = item.SoLuong,
                            DonGia = item.MaHh,
                            MaHh = item.MaHh,
                            GiamGia = 0
                        });
                    }

                    db.AddRange(cthds);

                    db.SaveChanges();

                    HttpContext.Session.Set<List<CartItem>>(MyConst.CART_KEY, new List<CartItem>());

                }
                catch
                {
                    db.Database.RollbackTransaction();
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }
        #endregion

        [Authorize]
        public IActionResult PaymentSuccess()
        {
            return View("Success");
        }


    }
}
