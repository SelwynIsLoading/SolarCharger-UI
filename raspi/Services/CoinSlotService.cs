using System.Text.Json;
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

    public async Task<ServiceResponse<bool>> Start()
    {
        var response = await client.PostAsync("/coins/start", null);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Dictionary<string, string>>(content);
            if (result != null && result.ContainsKey("status"))
            {
                if (result["status"] == "started")
                {
                    return ServiceResponse<bool>.SuccessResult(true);
                }
                
                if (result["status"] == "already_running")
                {
                    return ServiceResponse<bool>.FailureResult("Already running");
                }
                return ServiceResponse<bool>.SuccessResult(false);
            }
            else
            {
                return ServiceResponse<bool>.FailureResult("Invalid response from server.");
            }
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            return ServiceResponse<bool>.FailureResult($"Error: {response.StatusCode} - {errorContent}");
        }
    }
    
    public async Task<ServiceResponse<bool>> Stop()
    {
        var response = await client.PostAsync("/coins/stop", null);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Dictionary<string, string>>(content);
            if (result != null && result.ContainsKey("status"))
            {
                if (result["status"] == "stopped")
                {
                    return ServiceResponse<bool>.SuccessResult(true);
                }
                return ServiceResponse<bool>.SuccessResult(false);
            }
            return ServiceResponse<bool>.FailureResult("Invalid response from server.");
        }
        var errorContent = await response.Content.ReadAsStringAsync();
        return ServiceResponse<bool>.FailureResult($"Error: {response.StatusCode} - {errorContent}");
    }

    // public async Task<ServiceResponse<bool>> AddCoin()
    // {
    //     
    // }

    public class CoinSlotResponse
    {
        public int pulses { get; set; } 
    }
}