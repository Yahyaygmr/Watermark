using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using Watermark.Models;
using Watermark.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton(sp =>
{
    IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
    string rabbitMqUri = configuration.GetConnectionString("RabbitMQ"); // "RabbitMQ" burada baðlantý dizesi adýnýza göre ayarladýðýnýz isimdir.

    return new ConnectionFactory() { Uri = new Uri(rabbitMqUri) };
});
builder.Services.AddSingleton<RabbitMQClientService>();
builder.Services.AddSingleton<RabbitMQPublisher>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase(databaseName: "ProductDb");
});

builder.Services.AddControllersWithViews();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
