using Đồ_án.Areas.Admin.Models;
using Đồ_án.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Diagnostics;
using X.PagedList;

namespace Đồ_án.Controllers
{
    public class HomeController : Controller
    {
        private KhuTroContext _dbContext;
        private readonly IToastNotification toast;
        public HomeController(KhuTroContext dbContext, IToastNotification toast)
        {
            _dbContext = dbContext;
            this.toast = toast;
        }
        [Route("")]
        public IActionResult Index()
        {
            var userName = HttpContext.Session.GetString("username");
            var role = HttpContext.Session.GetString("role");
            if (userName != null)
            {
                if (role == "Admin")
                {
                    return View("~/Areas/Admin/Views/TaiKhoan/Index.cshtml", _dbContext.taiKhoan.AsNoTracking().ToPagedList(1, 10));
                }
                else if (role == "User")
                {
                    NguoiThue nguoiThue = _dbContext.nguoiThue.Where(x => x.CMND_CCCD == userName).FirstOrDefault();
                    return View("~/Views/ThongTinCaNhan/Index.cshtml", nguoiThue);
                    //D:\ISPACE\Web2\Đồ án\Đồ án\Views\ThongTinCaNhan\Index.cshtml
                }
            }
            return View("Login");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(TaiKhoan us)
        {
            if (ModelState.IsValid)
            {
                var objAD = _dbContext.taiKhoan.Where(a => a.taiKhoan.Equals(us.taiKhoan) &&
                                                    a.matKhau.Equals(us.matKhau)).FirstOrDefault();
                if (objAD != null)
                {
                    HttpContext.Session.SetString("role", "Admin");
                    HttpContext.Session.SetString("username", objAD.taiKhoan.ToString());
                    return RedirectToAction("Index");
                }
                var objUser = _dbContext.taikhoanNguoiDung.Where(a => a.CMND_CCCD.Equals(us.taiKhoan) &&
                                                    a.matKhau.Equals(us.matKhau)).FirstOrDefault();
                if (objUser != null)
                {
                    HttpContext.Session.SetString("role", "User");
                    HttpContext.Session.SetString("username", objUser.CMND_CCCD.ToString());
                    return RedirectToAction("Index");
                }
            }
            return View("Login");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("username");
            HttpContext.Session.Remove("role");
            return RedirectToAction("Index");
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string Password, string newPassword, string confirmPassword)
        {
            var userName = HttpContext.Session.GetString("username");
            var acccount = await _dbContext.taikhoanNguoiDung.Where(x => x.CMND_CCCD == userName).FirstOrDefaultAsync();
			if (Password == acccount.matKhau)
			{
				if (newPassword != Password && confirmPassword == newPassword)
				{
                    acccount.matKhau = newPassword;
                    await _dbContext.SaveChangesAsync();
                    toast.AddSuccessToastMessage("Đổi mật khẩu thành công", new ToastrOptions { Title = "Thông báo" });
					NguoiThue nguoiThue = _dbContext.nguoiThue.Where(x => x.CMND_CCCD == userName).FirstOrDefault();
					return View("~/Views/ThongTinCaNhan/Index.cshtml", nguoiThue);
				}
			}
			return View();
        }
        //[HttpPost]
        //public IActionResult ChangePassword(ChangePassword model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        return Redirect("/Admin/Home/login");
        //    }
        //    return View("ChangePassword", model);
        //}
    }
    public class ChangePassword
    {

    }
}