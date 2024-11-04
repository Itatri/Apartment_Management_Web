


// Các gói cài đặt : 
// Install - Package Microsoft.EntityFrameworkCore.SqlServer
// Install-Package Microsoft.EntityFrameworkCore.Tools	


// Generate Model from Database :
// Scaffold-DbContext "Server=TRIS72\VANTRI;Database=QL_ChungCu;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
//  Generate and Override Model from Database :
// Scaffold-DbContext "Server=TRIS72\VANTRI;Database=QL_ChungCu;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force


// Cài đặt gói xác thực JWT Bearer
// Install-Package Microsoft.AspNetCore.Authentication.JwtBearer
// dotnet add package System.IdentityModel.Tokens.Jwt

// Add libary Bootstrap Terminal 
// dotnet add package Bootstrap



// Add services to the container.

//  Đăng ký Service trong Program.cs

using Apartment_Management_Web.Interfaces;
using Apartment_Management_Web.Models;
using Apartment_Management_Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


//using Microsoft.AspNetCore.Mvc;



var builder = WebApplication.CreateBuilder(args);

// Thêm CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        builder => builder.WithOrigins("https://localhost:7230") // Địa chỉ của frontend
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});


// Đăng kí Interfacem Servues
builder.Services.AddScoped<IUserPhongService, UserPhongService>();
builder.Services.AddScoped<IThongTinKhachService, ThongTinKhachService>();
builder.Services.AddScoped<IPhieuThuService, PhieuThuService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();


builder.Services.AddControllers();

// Đọc cấu hình JWT từ appsettings.json
var jwtSettings = builder.Configuration.GetSection("Jwt");


// Cấu hình xác thực JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"], // Địa chỉ phát hành token hợp lệ (issuer)
            ValidAudience = jwtSettings["Audience"], // Đối tượng sử dụng token hợp lệ (audience)
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"])) // Chuỗi bảo mật token
        };
    });


// Sửa lại DBContext nếu có thay đổi DB
builder.Services.AddDbContext<QlChungCuContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

////builder.Services.AddControllersWithViews(); // Thêm dịch vụ cho MVC và View

var app = builder.Build();

// Sử dụng CORS kết nối frontend
app.UseCors("AllowOrigin");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Thêm UseAuthentication trước UseAuthorization
app.UseAuthentication();
app.UseAuthorization();



//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}"); // Thiết lập routing cho MVC

app.MapControllers();

app.Run();
