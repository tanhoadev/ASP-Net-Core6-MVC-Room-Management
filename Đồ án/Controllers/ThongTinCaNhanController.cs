using Đồ_án.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Đồ_án.Controllers
{
    public class ThongTinCaNhanController : Controller
    {
        public KhuTroContext context;
        private readonly IToastNotification toast;
        public ThongTinCaNhanController(KhuTroContext cc, IToastNotification toast)
        {
            context = cc;
            this.toast = toast;
        }
        public IActionResult Index()
        {
            var userName = HttpContext.Session.GetString("username");
            NguoiThue nguoiThue = context.nguoiThue.Where(x => x.CMND_CCCD == userName).FirstOrDefault();
            return View(nguoiThue);
        }
    }
}
