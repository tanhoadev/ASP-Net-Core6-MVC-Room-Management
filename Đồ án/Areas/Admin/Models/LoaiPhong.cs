namespace Đồ_án.Areas.Admin.Models
{
    public class LoaiPhong
    {
        public int Id { get; set; }
        public string? tenLoaiPhong { get; set; }
        public decimal? gia { get; set; }
        public int soNguoi { get; set; }
        public ICollection<Phong> phong { get; set; }
    }
}
