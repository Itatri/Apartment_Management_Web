using System;
using System.Collections.Generic;

namespace Apartment_Management_Web.Models;

public partial class DichVuPhieuThu
{
    public int Id { get; set; }

    public string? MaPt { get; set; }

    public string? TenDichVu { get; set; }

    public double? DonGia { get; set; }

    public virtual PhieuThu? MaPtNavigation { get; set; }
}
