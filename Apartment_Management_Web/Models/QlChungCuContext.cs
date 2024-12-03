using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Apartment_Management_Web.Models;

public partial class QlChungCuContext : DbContext
{
    public QlChungCuContext()
    {
    }

    public QlChungCuContext(DbContextOptions<QlChungCuContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DangNhap> DangNhaps { get; set; }

    public virtual DbSet<DichVu> DichVus { get; set; }

    public virtual DbSet<DichVuPhieuThu> DichVuPhieuThus { get; set; }

    public virtual DbSet<FeedBack> FeedBacks { get; set; }

    public virtual DbSet<KhuVuc> KhuVucs { get; set; }

    public virtual DbSet<PhieuThu> PhieuThus { get; set; }

    public virtual DbSet<Phong> Phongs { get; set; }

    public virtual DbSet<ThongTinAdmin> ThongTinAdmins { get; set; }

    public virtual DbSet<ThongTinKhach> ThongTinKhaches { get; set; }

    public virtual DbSet<TraPhong> TraPhongs { get; set; }

    public virtual DbSet<UserPhong> UserPhongs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=TRIS72\\VANTRI;Database=QL_ChungCu;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<DangNhap>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DangNhap__3214EC2733A4EC59");

            entity.ToTable("DangNhap");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.MaKhuVuc)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PassWord)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.MaKhuVucNavigation).WithMany(p => p.DangNhaps)
                .HasForeignKey(d => d.MaKhuVuc)
                .HasConstraintName("fk_DangNhap_KhuVuc");
        });

        modelBuilder.Entity<DichVu>(entity =>
        {
            entity.HasKey(e => e.MaDichVu).HasName("PK__DichVu__C0E6DE8F4B4DCD2D");

            entity.ToTable("DichVu", tb => tb.HasTrigger("trg_PreventDeleteDichVu"));

            entity.Property(e => e.MaDichVu)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DichVuPhieuThu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DichVuPh__3214EC270D289247");

            entity.ToTable("DichVuPhieuThu");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaPt)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("MaPT");

            entity.HasOne(d => d.MaPtNavigation).WithMany(p => p.DichVuPhieuThus)
                .HasForeignKey(d => d.MaPt)
                .HasConstraintName("fk_PhieuThu_DichVuPhieuThu");
        });

        modelBuilder.Entity<FeedBack>(entity =>
        {
            entity.HasKey(e => e.MaFb).HasName("PK__FeedBack__2725963C70304EB5");

            entity.ToTable("FeedBack");

            entity.Property(e => e.MaFb)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("MaFB");
            entity.Property(e => e.MaPhong)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NgayGui).HasColumnType("datetime");
            entity.Property(e => e.NgayPhanHoi).HasColumnType("datetime");

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.FeedBacks)
                .HasForeignKey(d => d.MaPhong)
                .HasConstraintName("fk_FeedBack_Phong");
        });

        modelBuilder.Entity<KhuVuc>(entity =>
        {
            entity.HasKey(e => e.MaKhuVuc).HasName("PK__KhuVuc__0676EB839ADAC0EC");

            entity.ToTable("KhuVuc");

            entity.Property(e => e.MaKhuVuc)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PhieuThu>(entity =>
        {
            entity.HasKey(e => e.MaPt).HasName("PK__PhieuThu__2725E7F632F0BC05");

            entity.ToTable("PhieuThu");

            entity.Property(e => e.MaPt)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("MaPT");
            entity.Property(e => e.MaPhong)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TienDv).HasColumnName("TienDV");

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.PhieuThus)
                .HasForeignKey(d => d.MaPhong)
                .HasConstraintName("fk_PhieuThu_Phong");
        });

        modelBuilder.Entity<Phong>(entity =>
        {
            entity.HasKey(e => e.MaPhong).HasName("PK__Phong__20BD5E5B5F4BE59A");

            entity.ToTable("Phong");

            entity.Property(e => e.MaPhong)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GhiChu).HasMaxLength(100);
            entity.Property(e => e.MaKhuVuc)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.MaKhuVucNavigation).WithMany(p => p.Phongs)
                .HasForeignKey(d => d.MaKhuVuc)
                .HasConstraintName("fk_Phong_KhuVuc");

            entity.HasMany(d => d.MaDichVus).WithMany(p => p.MaPhongs)
                .UsingEntity<Dictionary<string, object>>(
                    "DichVuPhong",
                    r => r.HasOne<DichVu>().WithMany()
                        .HasForeignKey("MaDichVu")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_DichVuPhong_DichVu"),
                    l => l.HasOne<Phong>().WithMany()
                        .HasForeignKey("MaPhong")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_DichVuPhong_Phong"),
                    j =>
                    {
                        j.HasKey("MaPhong", "MaDichVu").HasName("PK__DichVuPh__CCB333B3525F03C6");
                        j.ToTable("DichVuPhong");
                        j.IndexerProperty<string>("MaPhong")
                            .HasMaxLength(50)
                            .IsUnicode(false);
                        j.IndexerProperty<string>("MaDichVu")
                            .HasMaxLength(50)
                            .IsUnicode(false);
                    });
        });

        modelBuilder.Entity<ThongTinAdmin>(entity =>
        {
            entity.HasKey(e => e.MaAdmin).HasName("PK__ThongTin__49341E38FD562099");

            entity.ToTable("ThongTinAdmin");

            entity.Property(e => e.MaAdmin)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Cccd)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ChuKy).IsUnicode(false);
            entity.Property(e => e.GioiTinh).HasMaxLength(50);
            entity.Property(e => e.HoTen).HasMaxLength(50);
            entity.Property(e => e.IdUser)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NganHang).IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TaiKhoan)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.ThongTinAdmins)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("fk_ThongTinAdmin_DangNhap");
        });

        modelBuilder.Entity<ThongTinKhach>(entity =>
        {
            entity.HasKey(e => e.MaKhachTro).HasName("PK__ThongTin__297FACA660B7FA56");

            entity.ToTable("ThongTinKhach");

            entity.Property(e => e.MaKhachTro)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Cccd)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ChuKy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email).IsUnicode(false);
            entity.Property(e => e.GioiTinh).HasMaxLength(50);
            entity.Property(e => e.HoTen).HasMaxLength(50);
            entity.Property(e => e.MaPhong)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.QuanHe).HasMaxLength(50);

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.ThongTinKhaches)
                .HasForeignKey(d => d.MaPhong)
                .HasConstraintName("fk_ThongTinKhach_Phong");
        });

        modelBuilder.Entity<TraPhong>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TraPhong__3214EC27CE707E63");

            entity.ToTable("TraPhong");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.MaKhachTro)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MaPhong)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.MaKhachTroNavigation).WithMany(p => p.TraPhongs)
                .HasForeignKey(d => d.MaKhachTro)
                .HasConstraintName("fk_TraPhong_ThongTinKhach");

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.TraPhongs)
                .HasForeignKey(d => d.MaPhong)
                .HasConstraintName("fk_TraPhong_Phong");
        });

        modelBuilder.Entity<UserPhong>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserPhon__3214EC277908F3FF");

            entity.ToTable("UserPhong");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.MaPhong)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MatKhau)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.UserPhongs)
                .HasForeignKey(d => d.MaPhong)
                .HasConstraintName("fk_UserPhong_Phong");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
