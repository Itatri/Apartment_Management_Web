using System;
using System.Collections.Generic;

namespace Apartment_Management_Web.Models;

public partial class DichVu
{
    public string MaDichVu { get; set; } = null!;

    public string? TenDichVu { get; set; }

    public double? DonGia { get; set; }

    public bool? TrangThai { get; set; }

    public virtual ICollection<Phong> MaPhongs { get; set; } = new List<Phong>();
}
