using System;
using System.Collections.Generic;

namespace Apartment_Management_Web.Models;

public partial class TraPhong
{
    public string Id { get; set; } = null!;

    public string? MaKhachTro { get; set; }

    public string? MaPhong { get; set; }

    public DateOnly? NgayThue { get; set; }

    public DateOnly? NgayTra { get; set; }

    public virtual ThongTinKhach? MaKhachTroNavigation { get; set; }

    public virtual Phong? MaPhongNavigation { get; set; }
}
