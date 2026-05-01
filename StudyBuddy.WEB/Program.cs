using StudyBuddy.WEB.Models.Api;
using StudyBuddy.WEB.Services;
using StudyBuddy.WEB.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection("ApiSettings")
);

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IApiClientService, ApiClientService>();
builder.Services.AddScoped<IAuthWebService, AuthWebService>();
builder.Services.AddScoped<IUserWebService, UserWebService>();
builder.Services.AddScoped<IQuizWebService, QuizWebService>();
builder.Services.AddScoped<IMatchWebService, MatchWebService>();
builder.Services.AddScoped<IMatchRequestWebService, MatchRequestWebService>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
