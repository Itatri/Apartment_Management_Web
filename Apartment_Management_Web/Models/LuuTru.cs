using System;
using System.Collections.Generic;

namespace Apartment_Management_Web.Models;

public partial class LuuTru
{
    public string MaLuuTru { get; set; } = null!;

    public string? MaPhong { get; set; }

    public string? TenFile { get; set; }

    public int? TrangThai { get; set; }

    public virtual Phong? MaPhongNavigation { get; set; }
}
