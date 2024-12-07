


// Các gói cài đặt : 
// Install - Package Microsoft.EntityFrameworkCore.SqlServer
// Install-Package Microsoft.EntityFrameworkCore.Tools	


// Generate Model from Database :
// Scaffold-DbContext "Server=TRIS72\VANTRI;Database=QL_ChungCu;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models

//  Generate and Override Model from Database :
// Scaffold-DbContext "Server=TRIS72\VANTRI;Database=QL_ChungCu;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force

//  Triển khai IIS thì đổi Connection ở QLChungContext thành :
//  "Server=TRIS72\\VANTRI;Database=QL_ChungCu;User Id=ITApartment;Password=16092003;TrustServerCertificate=True;"



// Cài đặt gói xác thực JWT Bearer :
// Install-Package Microsoft.AspNetCore.Authentication.JwtBearer
// dotnet add package System.IdentityModel.Tokens.Jwt

// Cài đặt gói Bootstrap  
// dotnet add package Bootstrap

// Cài đặt Thư Viện iTextSharp ( PDF )
// dotnet add package itext7

// Cài Đặt Serilog ( Viết log lỗi )
// dotnet add package Serilog.AspNetCore
// dotnet add package Serilog.Sinks.File





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





var builder = WebApplication.CreateBuilder(args);


// Cấu hình  ghi log 
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/myapp.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();


builder.Host.UseSerilog();




// Lấy cấu hình FrontendUrl từ appsettings
var frontendUrl = builder.Configuration.GetValue<string>("FrontendUrl");


//// Thêm CORS ( bổ sung cho phép Frontend lấy Backend khi chạy trên Product )
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .WithExposedHeaders("Access-Control-Allow-Origin", "Access-Control-Allow-Methods", "Access-Control-Allow-Headers");
        });
});

// Cấu hình SmtpSettings từ appsettings.json
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

// Đăng ký dịch vụ EmailSender
builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();


// Đăng kí Interfacem Services
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
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
        };
    });


// Cấu hình kết nối Data
builder.Services.AddDbContext<QlChungCuContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

//// Chuyển hướng tới Swagger khi truy cập host API IIS
//app.Use(async (context, next) =>
//{
//    if (context.Request.Path == "/")
//    {
//        context.Response.Redirect("/swagger/index.html");
//    }
//    else
//    {
//        await next();
//    }
//});


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

app.UseAuthentication();
app.UseAuthorization();





app.MapControllers();

app.Run();
