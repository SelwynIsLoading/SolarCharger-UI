using raspi.DTOs;

namespace raspi.Services;

public class CoinSlotService(HttpClient client)
{
    public async Task<ServiceResponse<int>> GetCoinPulse()
    {
        var response = await client.GetAsync("/coins/wait");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<CoinSlotResponse>();
            if (result != null) return ServiceResponse<int>.SuccessResult(result.pulses);
        }
        return ServiceResponse<int>.FailureResult("Failed to get coin pulse");
    }

    public class CoinSlotResponse
    {
        public int pulses { get; set; } 
    }
}