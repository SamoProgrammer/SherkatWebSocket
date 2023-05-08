using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using SherkatWebSocket;
using SherkatWebSocket.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
var connectionString = "server=localhost;user=root;password=;database=sherkat";
var serverVersion = new MySqlServerVersion(new Version(8, 1, 17));
builder.Services.AddDbContext<DatabaseContext>(optionsBuilder =>
    optionsBuilder.UseMySql(connectionString, serverVersion));
builder.Services.AddSwaggerGen();
// builder.Services.AddSignalR();
builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
    RequestPath = "/Uploads"
});
var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(3)
};
app.UseWebSockets(webSocketOptions);
app.Use(async (context, next) =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        var buffer = new byte[1024 * 4];
        WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        while (!result.CloseStatus.HasValue)
        {
            string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            Console.WriteLine($"Received message: {message}");
            buffer = new byte[1024 * 4];
            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }
        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
    }
    else
    {
        await next();
    }
});

app.UseAuthorization();

app.MapControllers();
// app.MapHub<FilesHub>("/filesHub").RequireCors("CorsPolicy");

app.Run();
