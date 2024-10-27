using System;
using System.Collections.Generic;

namespace Apartment_Management_Web.Models;

public partial class ThongTinKhach
{
    public string MaKhachTro { get; set; } = null!;

    public string? HoTen { get; set; }

    public string? GioiTinh { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public string? Cccd { get; set; }

    public DateOnly? NgayCap { get; set; }

    public string? NoiCap { get; set; }

    public string? ThuongTru { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? QueQuan { get; set; }

    public string? QuanHe { get; set; }

    public string? ChuKy { get; set; }

    public string? MaPhong { get; set; }

    public int? TrangThai { get; set; }

    public virtual Phong? MaPhongNavigation { get; set; }

    public virtual ICollection<TraPhong> TraPhongs { get; set; } = new List<TraPhong>();
}
