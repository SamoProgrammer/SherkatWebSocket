using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SherkatWebSocketApi.Authentication;
using SherkatWebSocketApi.Authentication.Entities;
using SherkatWebSocketApi.Database;
using SherkatWebSocketApi.Entities;

namespace SherkatWebSocketApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class DevicesController : ControllerBase
{
    private readonly DatabaseContext _context;
    public DevicesController(DatabaseContext context)
    {
        _context = context;
    }
    [Authorize]
    [HttpPost("Register")]
    public async Task<IActionResult> Register(string deviceId,string location)
    {
        var username = User.Claims.First().Value;
        var admin = _context.Users.First(x => x.Username == username);
        var superAdmins = _context.Users.Where(x => x.Role == UserRoles.SuperAdmin).ToList();
        List<User> deviceAdmins = new List<User> { admin };
        deviceAdmins.AddRange(superAdmins);
        await _context.Devices.AddAsync(new Device()
        {
            Admins = deviceAdmins,
            DeviceId = deviceId,
            Location = location
        });
        return Ok();
    }
}