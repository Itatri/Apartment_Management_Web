using System;
using System.Collections.Generic;

namespace Apartment_Management_Web.Models;

public partial class UserPhong
{
    public string Id { get; set; } = null!;

    public string? MatKhau { get; set; }

    public string? MaPhong { get; set; }

    public int? TrangThai { get; set; }

    public virtual Phong? MaPhongNavigation { get; set; }
}
