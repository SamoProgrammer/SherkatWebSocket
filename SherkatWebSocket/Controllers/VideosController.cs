using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SherkatWebSocket.Database;
using SherkatWebSocket.Entites;

namespace SherkatWebSocket.Controllers;

[ApiController]
public class VideosController : Controller
{
    private readonly DatabaseContext _context;
    private readonly FilesHub _hub;

    public VideosController(DatabaseContext context, FilesHub hub)
    {
        _context = context;
        _hub = hub;
    }

    [HttpPost("/Videos/UploadVideo"), DisableRequestSizeLimit]
    public async Task<IActionResult> UploadVideo([FromForm] IFormFile file)
    {
        var folderName = Path.Combine("Uploads", "Videos");
        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

        if (file.Length == 0)
        {
            return BadRequest();
        }

        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
        var fullPath = Path.Combine(pathToSave, fileName);
        var dbPath = Path.Combine(folderName, fileName);

        if (await _context.Videos.AnyAsync(v => v.Name == fileName).ConfigureAwait(false))
        {
            return BadRequest("Video exists!");
        }

        await using (var stream = new FileStream(fullPath, FileMode.CreateNew))
        {
            await file.CopyToAsync(stream).ConfigureAwait(false);
        }

        var video = new Video
        {
            Code = new Random().Next(100, 10000),
            // Link = $"http://localhost:5000/Uploads/Videos/{dbPath}",
            Link = $"{Request.Host.Value}/Uploads/Videos/{dbPath}",
            Name = fileName
        };
        await _context.Videos.AddAsync(video).ConfigureAwait(false);
        await _hub.Clients.All
            .SendAsync(JsonConvert.SerializeObject(await _context.Videos.FirstAsync(v => v.Name == video.Name)
                .ConfigureAwait(false))).ConfigureAwait(false);
        return Ok(new { dbPath });
    }
}