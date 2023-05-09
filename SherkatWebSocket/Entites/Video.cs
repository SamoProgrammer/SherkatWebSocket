using System.ComponentModel.DataAnnotations;

namespace SherkatWebSocket.Entites;

public class Video
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Code { get; set; }
    [Url]
    public string Link { get; set; }
    public DateTime UploadTime { get; set; } = DateTime.Now;
}