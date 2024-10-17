using System;
using System.Collections.Generic;

namespace Apartment_Management_Web.Models;

public partial class KhuVuc
{
    public string MaKhuVuc { get; set; } = null!;

    public string? TenKhuVuc { get; set; }

    public bool? TrangThai { get; set; }

    public virtual ICollection<DangNhap> DangNhaps { get; set; } = new List<DangNhap>();

    public virtual ICollection<Phong> Phongs { get; set; } = new List<Phong>();
}
