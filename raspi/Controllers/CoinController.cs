using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using raspi.DTOs;
using raspi.Services.Models;

namespace raspi.Controllers
{
    [ApiController]
    [Route("api/coin")]
    public class CoinController : ControllerBase
    {
        private readonly IHubContext<CoinHub> _hubContext;
        private int coinTotal = 0;

        public CoinController(IHubContext<CoinHub> hubContext)
        {
            _hubContext = hubContext;
        }
        
        [HttpPost("inserted")]
        public async Task<IActionResult> CoinInserted([FromBody] CoinData data)
        { 
            await _hubContext.Clients.All.SendAsync("CoinInserted", new
            {
                data.Pulses,
            });

            return Ok();
        }
        

        private static readonly Dictionary<int, int> _pulseToSeconds = new()
        {
            { 1, 90 },
            { 5, 1200 },
            { 10, 1800 },
            { 20, 2400 }
        };

        private static int GetSeconds(int pulses)
        {
            return _pulseToSeconds.TryGetValue(pulses, out int seconds) ? seconds : 0;
        }
    }
    
    public class CoinData
    {
        public int Pulses { get; set; }
    }
}
