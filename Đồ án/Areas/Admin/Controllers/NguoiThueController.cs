using Đồ_án.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using NuGet.Protocol.Plugins;
using System.Drawing.Printing;
using X.PagedList;

namespace Đồ_án.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class NguoiThueController : Controller
	{
		public KhuTroContext context;
		private readonly IToastNotification toast;
        private bool CheckLogin()
        {
            if (HttpContext.Session.GetString("role") == "Admin")
            {
                return true;
            }
            return false;
        }
        public NguoiThueController(KhuTroContext cc, IToastNotification toast)
		{
			context = cc;
			this.toast = toast;
		}
		[Route("Admin/[controller]/[action]")]
		public IActionResult Index(int page = 1)
		{
            if (CheckLogin())
            {
				page = page < 1 ? 1 : page;
				int pageSize = 10;
				var tks = context.nguoiThue.AsNoTracking().ToPagedList(page, pageSize);
				return View(tks);
            }
            return View("~/Areas/Admin/Views/Share/PageNotFound.cshtml");
		}
		[Route("Admin/[controller]/[action]")]
		public IActionResult Create()
		{
            if (CheckLogin())
            {
				return View();
            }
            return View("~/Areas/Admin/Views/Share/PageNotFound.cshtml");
		}
		[Route("Admin/[controller]/[action]")]
		[HttpPost]
		public async Task<IActionResult> Create(NguoiThue tk,IFormFile anhDaiDien, IFormFile matTruocCCCD, IFormFile matSauCMND)
		{
			TaikhoanNguoiDung taikhoan = await context.taikhoanNguoiDung.Where(x => x.CMND_CCCD == tk.CMND_CCCD).FirstOrDefaultAsync();
			if (taikhoan == null)
			{
				taikhoan = new TaikhoanNguoiDung { CMND_CCCD = tk.CMND_CCCD, matKhau = tk.CMND_CCCD };
				context.Add(taikhoan);
				context.SaveChanges();
				toast.AddSuccessToastMessage("Đã thêm 1 tài khoản người dùng", new ToastrOptions { Title = "Thông báo" });
			}
			tk.anhDaiDien = Path.GetFileName(anhDaiDien.FileName);
			tk.matTruocCCCD = Path.GetFileName(matTruocCCCD.FileName);
			tk.matSauCMND = Path.GetFileName(matSauCMND.FileName);
			string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "admin", "Upload"));
			using (var filestream = new FileStream(Path.Combine(path, tk.anhDaiDien), FileMode.Create))
			{
				await anhDaiDien.CopyToAsync(filestream);
			}
			using (var filestream = new FileStream(Path.Combine(path, tk.matTruocCCCD), FileMode.Create))
			{
				await matTruocCCCD.CopyToAsync(filestream);
			}
			using (var filestream = new FileStream(Path.Combine(path, tk.matSauCMND), FileMode.Create))
			{
				await matSauCMND.CopyToAsync(filestream);
			}
			context.Add(tk);
			await context.SaveChangesAsync();
			toast.AddSuccessToastMessage("Thêm thành công người thuê mới", new ToastrOptions { Title = "Thông báo" });
			return RedirectToAction("Index");
		}
		[Route("Admin/[controller]/[action]")]
		public async Task<IActionResult> Update(int id)
		{
			NguoiThue tk = await context.nguoiThue.Where(e => e.Id == id).FirstOrDefaultAsync();
			ViewBag.anhDaiDien = tk.anhDaiDien;
			ViewBag.matTruoc = tk.matTruocCCCD;
			ViewBag.matSau = tk.matSauCMND;
			return View(tk);
		}
		[HttpPost]
		[Route("Admin/[controller]/[action]")]
		public async Task<IActionResult> Update(NguoiThue tk, string CCCD, string[] listAnh, IFormFile anhDaiDien, IFormFile matTruocCCCD, IFormFile matSauCMND)
		{
            if (CheckLogin())
            {
				TaikhoanNguoiDung taikhoan = await context.taikhoanNguoiDung.Where(x => x.CMND_CCCD == CCCD).FirstOrDefaultAsync();
				if(taikhoan != null)
				{
					taikhoan.CMND_CCCD = tk.CMND_CCCD;
					taikhoan.matKhau = tk.CMND_CCCD;
					context.Update(taikhoan);
					context.SaveChanges();
					toast.AddSuccessToastMessage("Cập nhật thành công tài khoản người thuê", new ToastrOptions { Title = "Thông báo" });
				}
				string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "admin", "Upload"));
				if(anhDaiDien != null && anhDaiDien.Length > 0)
				{
					tk.anhDaiDien = Path.GetFileName(anhDaiDien.FileName);
					using (var filestream = new FileStream(Path.Combine(path, tk.anhDaiDien), FileMode.Create))
					{
						await anhDaiDien.CopyToAsync(filestream);
					}
				}
				else
				{
					tk.anhDaiDien = listAnh[0];
				}
				if(matTruocCCCD != null && matTruocCCCD.Length > 0)
				{
					tk.matTruocCCCD = Path.GetFileName(matTruocCCCD.FileName);
					using (var filestream = new FileStream(Path.Combine(path, tk.matTruocCCCD), FileMode.Create))
					{
						await matTruocCCCD.CopyToAsync(filestream);
					}
				}
				else
				{
					tk.matTruocCCCD = listAnh[1];
				}
				if(matSauCMND != null && matSauCMND.Length > 0)
				{
					tk.matSauCMND = Path.GetFileName(matSauCMND.FileName);
					using (var filestream = new FileStream(Path.Combine(path, tk.matSauCMND), FileMode.Create))
					{
						await matSauCMND.CopyToAsync(filestream);
					}
				}
				else
				{
					tk.matSauCMND = listAnh[2];
				}
				context.Update(tk);
				await context.SaveChangesAsync();
				toast.AddSuccessToastMessage("Cập nhật thành công người thuê", new ToastrOptions { Title = "Thông báo" });
				return RedirectToAction("Index");
            }
            return View("~/Areas/Admin/Views/Share/PageNotFound.cshtml");
		}
		[HttpPost]
		[Route("Admin/[controller]/[action]")]
		public async Task<IActionResult> Delete(int id, string CMND)
		{
			try
			{
				var tk = new NguoiThue() { Id = id};
				context.Remove(tk);
				await context.SaveChangesAsync();
				toast.AddSuccessToastMessage("Xóa thành công người thuê", new ToastrOptions { Title = "Thông báo" });

				//Sẽ lỗi nêu tạo 2 thực thể giống nhau và cùng ID
				TaikhoanNguoiDung taikhoan = context.taikhoanNguoiDung.Where(x => x.CMND_CCCD == CMND).FirstOrDefault();
				if (taikhoan != null)
				{
					context.Remove(taikhoan);
					context.SaveChanges();
					toast.AddSuccessToastMessage("Xóa thành công tài khoản người thuê", new ToastrOptions { Title = "Thông báo" });
				}
			}
			catch
			{
				
				toast.AddErrorToastMessage("Không thể xóa người thuê này", new ToastrOptions { Title = "Thông báo" });
			}

			return RedirectToAction("Index");
		}
		[HttpPost]
		[Route("Admin/[controller]/[action]")]
		public async Task<IActionResult> Result(int id)
		{
			var thuePhong = await context.thuePhong.Include(x => x.phong).Include(x => x.nguoiThue).Where(x => x.IDPhong == id).ToListAsync();
			List<int> IDNguoiThue = new List<int>();
			foreach(var nguoi in thuePhong)
			{
				IDNguoiThue.Add(nguoi.IDNguoiThue);
			}
			var nguoiThue = await context.nguoiThue.Where(x =>  IDNguoiThue.Contains(x.Id)).ToListAsync();
			return View("Index", nguoiThue.ToPagedList(1, 10));
		}
		[HttpPost]
		[Route("Admin/[controller]/[action]")]
		public async Task<IActionResult> Search(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return View("Index", context.nguoiThue.ToList().ToPagedList(1, 10));
			}
			var nguoiThue = await context.nguoiThue.Where(x => x.hoTen.ToLower().Contains(name.ToLower())).ToListAsync();
			return View("Index", nguoiThue.ToPagedList(1, 10));
		}
	}
}
