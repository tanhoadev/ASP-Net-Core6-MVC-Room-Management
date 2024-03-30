using Đồ_án.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Đồ_án.Controllers
{
    public class ThongBaoController : Controller
    {
        public KhuTroContext context;
        private readonly IToastNotification toast;
        public ThongBaoController(KhuTroContext cc, IToastNotification toast)
        {
            context = cc;
            this.toast = toast;
        }
        public IActionResult Index()
        {
            var ThongBao = context.thongBao.ToList();
            ViewBag.ThongBao = ThongBao;
            return View(ThongBao);
        }
        public IActionResult Details(int id)
        {
            var ThongBao = context.thongBao.Where(x => x.Id == id).FirstOrDefault();
            return PartialView("_ThongBaoPartialView", ThongBao);
        }
    }
}
