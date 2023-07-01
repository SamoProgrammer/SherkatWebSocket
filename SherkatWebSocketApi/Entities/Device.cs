using SherkatWebSocketApi.Authentication.Entities;

namespace SherkatWebSocketApi.Entities;

public class Device
{
    public string DeviceId { get; set; }
    public virtual ICollection<User> Admins { get; set; }
    public string Location { get; set; }
}