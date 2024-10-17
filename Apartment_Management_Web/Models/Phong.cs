using System;
using System.Collections.Generic;

namespace Apartment_Management_Web.Models;

public partial class Phong
{
    public string MaPhong { get; set; } = null!;

    public string? MaKhuVuc { get; set; }

    public string? TenPhong { get; set; }

    public DateOnly? NgayVao { get; set; }

    public double? TienCoc { get; set; }

    public double? TienPhong { get; set; }

    public double? Dien { get; set; }

    public double? Nuoc { get; set; }

    public double? CongNo { get; set; }

    public DateOnly? HanTro { get; set; }

    public bool? TrangThai { get; set; }

    public string? GhiChu { get; set; }

    public virtual ICollection<FeedBack> FeedBacks { get; set; } = new List<FeedBack>();

    public virtual ICollection<LuuTru> LuuTrus { get; set; } = new List<LuuTru>();

    public virtual KhuVuc? MaKhuVucNavigation { get; set; }

    public virtual ICollection<PhieuThu> PhieuThus { get; set; } = new List<PhieuThu>();

    public virtual ICollection<ThongTinKhach> ThongTinKhaches { get; set; } = new List<ThongTinKhach>();

    public virtual ICollection<TraPhong> TraPhongs { get; set; } = new List<TraPhong>();

    public virtual ICollection<UserPhong> UserPhongs { get; set; } = new List<UserPhong>();

    public virtual ICollection<DichVu> MaDichVus { get; set; } = new List<DichVu>();
}
