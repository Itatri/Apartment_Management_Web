using System;
using System.Collections.Generic;

namespace Apartment_Management_Web.Models;

public partial class PhieuThu
{
    public string MaPt { get; set; } = null!;

    public string? MaPhong { get; set; }

    public DateOnly? NgayLap { get; set; }

    public DateOnly? NgayThu { get; set; }

    public double? TienNha { get; set; }

    public double? DienCu { get; set; }

    public double? DienMoi { get; set; }

    public double? TienDien { get; set; }

    public double? NuocCu { get; set; }

    public double? NuocMoi { get; set; }

    public double? TienNuoc { get; set; }

    public double? TienDv { get; set; }

    public double? TongTien { get; set; }

    public double? ThanhToan { get; set; }

    public bool? TrangThai { get; set; }

    public virtual ICollection<DichVuPhieuThu> DichVuPhieuThus { get; set; } = new List<DichVuPhieuThu>();

    public virtual Phong? MaPhongNavigation { get; set; }
}
