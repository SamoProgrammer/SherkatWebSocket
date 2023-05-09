using Microsoft.AspNetCore.SignalR;

namespace SherkatWebSocket;

public class FilesHub : Hub
{
    public async Task SendMessage(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }
}