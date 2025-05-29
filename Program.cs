using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// ↓ HTTPS強制リダイレクトを有効化していない（重要！）
var app = builder.Build();

// ↓ ここでリクエストのプロトコル確認（ログ用）
app.Use(async (context, next) =>
{
    var proto = context.Request.Headers["X-Forwarded-Proto"].FirstOrDefault();
    Console.WriteLine($"[アクセスログ] プロトコル: {proto}, パス: {context.Request.Path}");

    await next.Invoke();
});

// ↓ 単純なテキスト応答
app.MapGet("/", () => "Hello from .NET 8 on HTTP!");

app.Run();
