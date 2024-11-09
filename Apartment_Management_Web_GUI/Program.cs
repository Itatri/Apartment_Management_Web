var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Lấy URL API từ appsettings.json
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"];

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=HomePage}/{id?}");

app.MapControllerRoute(name: "inputBill",  // Tên của route
    pattern: "user/input",  // URL mà người dùng sẽ truy cập
    defaults: new { controller = "Bill", action = "InputBill" }  // Điều hướng đến action và controller tương ứng
);

app.MapControllerRoute(name: "userProfile",  // Tên của route
    pattern: "user/profile",  // URL mà người dùng sẽ truy cập
    defaults: new { controller = "Login", action = "UserProfile" }  // Điều hướng đến action và controller tương ứng
);


app.MapControllerRoute(name: "feedbackPage",  // Tên của route
    pattern: "user/feedback",  // URL mà người dùng sẽ truy cập
    defaults: new { controller = "Feedback", action = "FeedbackPage" }  // Điều hướng đến action và controller tương ứng
);


app.MapControllerRoute(name: "homePageLogin",  // Tên của route
    pattern: "user/homepage",  // URL mà người dùng sẽ truy cập
    defaults: new { controller = "Home", action = "HomePageLogin" }  // Điều hướng đến action và controller tương ứng
);



app.MapControllerRoute(name: "aboutUs",  // Tên của route
    pattern: "home/aboutus",  // URL mà người dùng sẽ truy cập
    defaults: new { controller = "About", action = "AboutPages" }  // Điều hướng đến action và controller tương ứng
);


app.MapControllerRoute(name: "loginPage",  // Tên của route
    pattern: "home/login",  // URL mà người dùng sẽ truy cập
    defaults: new { controller = "Login", action = "LoginPages" }  // Điều hướng đến action và controller tương ứng
);


app.MapControllerRoute(name: "homePage",  // Tên của route
    pattern: "home/homepage",  // URL mà người dùng sẽ truy cập
    defaults: new { controller = "Home", action = "HomePage" }  // Điều hướng đến action và controller tương ứng
);

app.Run();
