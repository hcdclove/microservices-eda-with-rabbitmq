using Microsoft.AspNetCore.SignalR;

namespace SignalRTelemetry.Hubs;

public class TelemetryHub : Hub  
{
    public async Task SendMessage(String user, string message)  {
        await Clients.All.SendAsync("TelemetryReceived", user, message);
    }
}