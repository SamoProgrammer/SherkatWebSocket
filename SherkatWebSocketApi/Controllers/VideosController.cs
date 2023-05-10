using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

// using Microsoft.Net.Http.Headers;

namespace SherkatWebSocketApi.Controllers;

[ApiController]
public class VideosController : ControllerBase
{
    private static readonly List<WebSocket> connectedClients = new List<WebSocket>();

    [HttpGet("notification")]
    public async Task Notification()
    {
        var context = ControllerContext.HttpContext;
        if (context.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            connectedClients.Add(webSocket);
            while (webSocket.State == WebSocketState.Open)
            {
                // receive incoming messages and handle them here
            }

            connectedClients.Remove(webSocket);
        }
        else
        {
            context.Response.StatusCode = 400;
        }
    }

    [HttpPost("/upload")]
    public async Task<IActionResult> Upload([FromForm]Video video)
    {
        // save the file

        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "Videos");
        if (video.VideoFile.Length > 0)
        {
            var fileName = video.VideoFile.FileName;
            var fullPath = Path.Combine(pathToSave, fileName);
            await using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await video.VideoFile.CopyToAsync(stream);
            }

            // send notification message to connected clients
            string message = $"http://localhost:5034/Videos/{video.VideoFile.FileName}";
            var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
            var tasks = new List<Task>();
            foreach (var webSocket in connectedClients)
            {
                if (webSocket.State == WebSocketState.Open)
                {
                    tasks.Add(webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None));
                }
            }

            Console.WriteLine(message);
            await Task.WhenAll(tasks);

            return Ok();
        }

        return BadRequest();
    }
}