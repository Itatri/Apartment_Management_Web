using System;
using System.Collections.Generic;

namespace Apartment_Management_Web.Models;

public partial class DangNhap
{
    public string Id { get; set; } = null!;

    public string? PassWord { get; set; }

    public string? MaKhuVuc { get; set; }

    public bool? TrangThai { get; set; }

    public virtual KhuVuc? MaKhuVucNavigation { get; set; }

    public virtual ICollection<ThongTinAdmin> ThongTinAdmins { get; set; } = new List<ThongTinAdmin>();
}
