


// Các gói cài đặt : 
// Install - Package Microsoft.EntityFrameworkCore.SqlServer
// Install-Package Microsoft.EntityFrameworkCore.Tools	


// Generate Model from Database :
// Scaffold-DbContext "Server=TRIS72\VANTRI;Database=QL_ChungCu_08_10_2024;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models

// Cài đặt gói xác thực JWT Bearer
// Install-Package Microsoft.AspNetCore.Authentication.JwtBearer
// dotnet add package System.IdentityModel.Tokens.Jwt


// Add services to the container.

//  Đăng ký Service trong Program.cs

using Apartment_Management_Web.Models;
using Apartment_Management_Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUserPhongService, UserPhongService>();


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



builder.Services.AddDbContext<QlChungCu08102024Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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

app.MapControllers();

app.Run();
