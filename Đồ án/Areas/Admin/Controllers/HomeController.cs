//using Đồ_án.Areas.Admin.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Drawing.Printing;
//using X.PagedList;

//namespace Đồ_án.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    public class HomeController : Controller
//    {
//        private KhuTroContext _dbContext;
//        public HomeController(KhuTroContext dbContext)
//        {
//            _dbContext = dbContext;
//        }
//        [Route("Admin/[controller]/[action]")]
//        public IActionResult Index()
//        {
//            var userName = HttpContext.Session.GetString("username");
//            var role = HttpContext.Session.GetString("role");
//            if (userName != null)
//            {
//                if(role == "Admin")
//                {
//                    return View("~/Areas/Admin/Views/TaiKhoan/Index.cshtml", _dbContext.taiKhoan.AsNoTracking().ToPagedList(1, 10));
//                }
//                else if(role == "User")
//                {
//					return View("~/Areas/User/Views/Home/Index.cshtml");
//				}
//            }
//            return View("Login");
//        }
//        [Route("Admin/[controller]/[action]")]
//        public IActionResult Login()
//        {
//            return View();  
//        }

//        [HttpPost]
//        [Route("Admin/[controller]/[action]")]
//        public IActionResult Login(TaiKhoan us) {
//            if (ModelState.IsValid)
//            {
//                var objAD = _dbContext.taiKhoan.Where(a => a.taiKhoan.Equals(us.taiKhoan) &&
//                                                    a.matKhau.Equals(us.matKhau)).FirstOrDefault();
//                if(objAD != null)
//                {
//                    HttpContext.Session.SetString("role", "Admin");
//                    HttpContext.Session.SetString("username", objAD.taiKhoan.ToString());
//                    return RedirectToAction("Index");
//                }
//                var objUser = _dbContext.taikhoanNguoiDung.Where(a => a.CMND_CCCD.Equals(us.taiKhoan) &&
//                                                    a.matKhau.Equals(us.matKhau)).FirstOrDefault();
//                if(objUser != null)
//                {
//					HttpContext.Session.SetString("role", "User");
//					HttpContext.Session.SetString("username", objUser.CMND_CCCD.ToString());
//                    return View("~/Areas/User/Views/Home/Index.cshtml");
//                }
//            }
//            return View("Login"); 
//        }
//    }
//}
