﻿using Apartment_Management_Web.Models;
using Apartment_Management_Web.Models.Bill;

namespace Apartment_Management_Web.Interfaces
{
    public interface IPhieuThuService
    {
        Task<IEnumerable<PhieuThu>> GetAllThongTinPhieuThuAsync();

        Task<List<PhieuThu>> GetThongTinPhieuThuBy_MaPhongAsync(string maPhong, DateOnly? startDate, DateOnly? endDate, bool? trangThai, int pageNumber, int pageSize);
        Task<int> GetTotalCountAsync(string maPhong, DateOnly? startDate, DateOnly? endDate, bool? trangThai);

        Task<PhieuThu?> GetPhieuThuByMaPtAsync(string maPt);
        Task<bool> UpdatePhieuThuAsync(PhieuThu phieuThu);

        Task<byte[]> ExportPhieuThuToPdfAsync(string maPt);

        Task<AdminInfoResponse> GetAdminInfoByMaPhongAsync(string maPhong);
    }
}
