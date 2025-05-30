using Microsoft.EntityFrameworkCore;
using YourNamespace.Data;
using YourNamespace.Models;

var builder = WebApplication.CreateBuilder(args);

// appsettings.json から読み込む
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// DbContext の登録
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// データベースのマイグレーションを適用
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// エンドポイントの定義
app.MapGet("/", () => "Hello World!");

app.MapGet("/customers", async (AppDbContext db) =>
    await db.Customers.ToListAsync());

app.MapPost("/customers", async (AppDbContext db, Customer customer) =>
{
    db.Customers.Add(customer);
    await db.SaveChangesAsync();
    return Results.Created($"/customers/{customer.Id}", customer);
});

app.Run();
