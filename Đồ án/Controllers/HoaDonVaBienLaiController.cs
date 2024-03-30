using Đồ_án.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Đồ_án.Controllers
{
	public class HoaDonVaBienLaiController : Controller
	{
        public KhuTroContext context;
        private readonly IToastNotification toast;
        public HoaDonVaBienLaiController(KhuTroContext cc, IToastNotification toast)
        {
            context = cc;
            this.toast = toast;
        }
        public IActionResult Index()
		{
            HoaDon x= new HoaDon();
            //x.ngayThanhToan
            var userName = HttpContext.Session.GetString("username");
            var chuaThanhToan = context.hoaDon.Where(x => x.trangThai == 0 && x.CMND_CCCD == userName).ToList();
            var daThanhToan = context.hoaDon.Where(x => x.trangThai == 1 && x.CMND_CCCD == userName).ToList();
            var choDuyet = context.hoaDon.Where(x => x.trangThai == 2 && x.CMND_CCCD == userName).ToList();
            ViewBag.ChuaThanhToan = chuaThanhToan;
            ViewBag.DaThanhToan = daThanhToan;
            ViewBag.ChoDuyet = choDuyet;
			return View();
		}
        public IActionResult Details(int id)
        {
            var HoaDon = context.hoaDon.Where(x => x.Id == id).FirstOrDefault();
			string[] dvsd = HoaDon.DSdichVuSuDung.Split(new string[] { "</>" }, StringSplitOptions.RemoveEmptyEntries);
			string[] giadvsd = HoaDon.DSGiaDichVuSuDung.Split(new string[] { "</>" }, StringSplitOptions.RemoveEmptyEntries);
			List<string> dichVuSuDung = new List<string>(dvsd);
			List<string> giaDichVuSuDung = new List<string>(giadvsd);
			ViewBag.DSDV = dichVuSuDung;
			ViewBag.GiaDVSD = giaDichVuSuDung;
			return PartialView("_DetailsHoaDscsconPartialView", HoaDon);
        }
        public IActionResult ThanhToan(int id)
        {
            var HoaDon = context.hoaDon.Where(x => x.Id == id).FirstOrDefault();
            return PartialView("_ThanhToanPartialView", HoaDon);
        }
	}
}
