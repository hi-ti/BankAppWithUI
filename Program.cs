
using MVCApp1.Models;
using MVCApp1.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddSingleton<FileServices>();
builder.Services.AddSingleton<Authentication>();
//builder.Services.AddScoped<Operations>();
//builder.Services.AddSingleton<PinValidation>();
builder.Services.AddScoped<TransactionService>();
//builder.Services.Add


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
