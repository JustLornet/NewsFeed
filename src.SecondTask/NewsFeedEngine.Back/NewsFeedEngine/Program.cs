using Microsoft.EntityFrameworkCore;
using NewsFeedEngine.DataAccess;
using NewsFeedEngine.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// добавляем CORS
builder.Services.AddCors();

// извлечение строки подключения к БД из config
var connectionString = Config.GetConnectionString();
// добавление и настройка БД
builder.Services.AddDbContext<Db>((options) => options.UseSqlServer(connectionString));

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

// настраиваем CORS
// TODO: сейчас установлено - для любых источников, далее - настроить
app.UseCors((cors) => cors.AllowAnyOrigin());

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();