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


// Set Router
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=HomePage}/{id?}");

app.MapControllerRoute(name: "inputBill",
    pattern: "user/input",
    defaults: new { controller = "Bill", action = "InputBill" }
);

app.MapControllerRoute(name: "userProfile",
    pattern: "user/profile",
    defaults: new { controller = "Login", action = "UserProfile" }
);


app.MapControllerRoute(name: "feedbackPage",
    pattern: "user/feedback",
    defaults: new { controller = "Feedback", action = "FeedbackPage" }
);


app.MapControllerRoute(name: "homePageLogin",
    pattern: "user/homepage",
    defaults: new { controller = "Home", action = "HomePageLogin" }
);



app.MapControllerRoute(name: "aboutUs",
    pattern: "home/aboutus",
    defaults: new { controller = "About", action = "AboutPages" }
);


app.MapControllerRoute(name: "loginPage",
    pattern: "home/login",
    defaults: new { controller = "Login", action = "LoginPages" }
);


app.MapControllerRoute(name: "homePage",
    pattern: "home/homepage",
    defaults: new { controller = "Home", action = "HomePage" }
);

app.Run();
