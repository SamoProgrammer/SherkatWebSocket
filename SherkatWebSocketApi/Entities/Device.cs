using SherkatWebSocketApi.Authentication.Entities;

namespace SherkatWebSocketApi.Entities;

public class Device
{
    public string DeviceId { get; set; }
    public ICollection<User> Admins { get; set; }
    public string Location { get; set; }
}