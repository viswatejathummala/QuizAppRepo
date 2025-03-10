using Microsoft.AspNetCore.Localization;
using QuizAppWeb.Service;
using Serilog;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Set up Serilog as the logging provider
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day) 
    .CreateLogger();


builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true; 
    options.Cookie.IsEssential = true;
});

// Register IHttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpClient<IApiService, ApiService>();
builder.Services.AddScoped<IApiService, ApiService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Configure Supported Cultures
var supportedCultures = CultureInfo.GetCultures(CultureTypes.AllCultures)
                                    .Where(c => !string.IsNullOrEmpty(c.Name)) 
                                    .ToList();
var localizationOptions = new RequestLocalizationOptions()
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseRequestLocalization(localizationOptions);


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
