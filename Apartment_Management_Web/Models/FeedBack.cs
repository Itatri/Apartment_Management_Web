using System;
using System.Collections.Generic;

namespace Apartment_Management_Web.Models;

public partial class FeedBack
{
    public string MaFb { get; set; } = null!;

    public string? MaPhong { get; set; }

    public string? MoTa { get; set; }

    public DateTime? NgayGui { get; set; }

    public string? PhanHoi { get; set; }

    public DateTime? NgayPhanHoi { get; set; }

    public int? TrangThai { get; set; }

    public virtual Phong? MaPhongNavigation { get; set; }
}
