using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using raspi.Services.Models;

namespace raspi.Controllers
{
    [Route("api/fingerprint")]
    [ApiController]
    public class FingerprintController : ControllerBase
    {
        private readonly IHubContext<FingerprintHub> _hubContext;

        public FingerprintController(IHubContext<FingerprintHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public class IncomingFingerprintPayload
        {
            public string Event { get; set; } = string.Empty;
            public FingerprintData Data { get; set; } = new();
        }

        public class FingerprintData
        {
            public int Id { get; set; } 
            public string Message { get; set; } = string.Empty;
        }
        
        [HttpPost("detected")]
        public async Task<IActionResult> Post([FromBody] IncomingFingerprintPayload payload)
         {
            var fullEvent = new FullFingerprintEvent
            {
                EventType = payload.Event,
                UserId = payload.Data.Id,
                Message = payload.Data.Message
            };

            await _hubContext.Clients.All.SendAsync("FingerprintDetected", fullEvent);
            return Ok();
        }
        
        public class FullFingerprintEvent
        {
            public string EventType { get; set; } = string.Empty;
            public int UserId { get; set; } 
            public string Message { get; set; } = string.Empty;
        }
    }
}
