using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRTelemetry.Hubs;

namespace SignalRTelemetry.Controllers;

[ApiController]
[Route("[controller]")]
public class TelemetryController : ControllerBase
{
    private readonly ILogger<TelemetryController> _logger;
    private readonly IHubContext<TelemetryHub> _hubContext;

    public TelemetryController(ILogger<TelemetryController> logger, IHubContext<TelemetryHub> hubContext)
    {
        _logger = logger;
        _hubContext=hubContext;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TelemetryData telemetry)    {
        
        await _hubContext.Clients.All.SendAsync("TelemetryReceived", telemetry.Decibels);
        return Ok();
    }

    public void SendSignalRNotification(TelemetryData telemetry)  {
        _hubContext.Clients.All.SendAsync("TelemetryReceived", telemetry.Decibels);
    }
}
