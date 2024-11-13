


// Các gói cài đặt : 
// Install - Package Microsoft.EntityFrameworkCore.SqlServer
// Install-Package Microsoft.EntityFrameworkCore.Tools	


// Generate Model from Database :
// Scaffold-DbContext "Server=TRIS72\VANTRI;Database=QL_ChungCu;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models

//  Generate and Override Model from Database :
// Scaffold-DbContext "Server=TRIS72\VANTRI;Database=QL_ChungCu;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force

//  Triển khai IIS thì đổi Connection ở QLChungContext thành "Server=TRIS72\\VANTRI;Database=QL_ChungCu;User Id=ITApartment;Password=16092003;TrustServerCertificate=True;"



// Cài đặt gói xác thực JWT Bearer
// Install-Package Microsoft.AspNetCore.Authentication.JwtBearer
// dotnet add package System.IdentityModel.Tokens.Jwt

// Add libary Bootstrap Terminal 
// dotnet add package Bootstrap

// Cài đặt Thư Viện iTextSharp ( Chuyen den thu muc cua API ) 
// dotnet add package itext7

// Cài Đặt Serilog
// dotnet add package Serilog.AspNetCore
// dotnet add package Serilog.Sinks.File




// Add services to the container.

//  Đăng ký Service trong Program.cs

using Apartment_Management_Web.Interfaces;
using Apartment_Management_Web.Models;
using Apartment_Management_Web.Models.Authentication;
using Apartment_Management_Web.Models.Mail;
using Apartment_Management_Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;


//using Microsoft.AspNetCore.Mvc;



var builder = WebApplication.CreateBuilder(args);


// Cấu hình Serilog để ghi log vào file
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console() // Ghi log vào console
    .WriteTo.File("Logs/myapp.txt", rollingInterval: RollingInterval.Day) // Ghi log vào file (lưu theo ngày)
    .CreateLogger();

// Đăng ký Serilog làm logger cho ứng dụng
builder.Host.UseSerilog(); // Đảm bảo sử dụng Serilog




// Lấy cấu hình FrontendUrl từ appsettings
var frontendUrl = builder.Configuration.GetValue<string>("FrontendUrl");

// Thêm CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        builder => builder.WithOrigins(frontendUrl) // Sử dụng URL từ cấu hình
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

// Cấu hình SmtpSettings từ appsettings.json
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

// Đăng ký dịch vụ EmailSender
builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();


// Đăng kí Interfacem Servues
builder.Services.AddScoped<IUserPhongService, UserPhongService>();
builder.Services.AddScoped<IThongTinKhachService, ThongTinKhachService>();
builder.Services.AddScoped<IPhieuThuService, PhieuThuService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();



builder.Services.AddControllers();

// Đọc cấu hình JWT từ appsettings.json
var jwtSettings = builder.Configuration.GetSection("Jwt");
// Đăng ký dịch vụ dùng chung cho các Controller
builder.Services.Configure<JwtSettings>(jwtSettings);

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

// Fix lỗi không Deploy được API trên IIS 
app.UseSwagger();
app.UseSwaggerUI();
// --------------------------------------

app.UseHttpsRedirection();

// Thêm UseAuthentication trước UseAuthorization
app.UseAuthentication();
app.UseAuthorization();



//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}"); // Thiết lập routing cho MVC

app.MapControllers();

app.Run();
