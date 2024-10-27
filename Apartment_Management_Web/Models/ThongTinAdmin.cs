using System;
using System.Collections.Generic;

namespace Apartment_Management_Web.Models;

public partial class ThongTinAdmin
{
    public string MaAdmin { get; set; } = null!;

    public string? HoTen { get; set; }

    public string? GioiTinh { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public string? Cccd { get; set; }

    public string? DiaChi { get; set; }

    public string? Phone { get; set; }

    public string? ChuKy { get; set; }

    public string? IdUser { get; set; }

    public int? TrangThai { get; set; }

    public virtual DangNhap? IdUserNavigation { get; set; }
}
