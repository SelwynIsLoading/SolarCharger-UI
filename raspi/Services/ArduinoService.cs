using System.Text;
using System.Text.Json;
using raspi.DTOs;

namespace raspi.Services;

public class ArduinoService(HttpClient client)
{
    public async Task<ServiceResponse<bool>> SendCommand(string command)
    {
        var payload = new { command = command };
        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("/arduino/send", content);

        if (response.IsSuccessStatusCode)
        {
            
            return ServiceResponse<bool>.SuccessResult(true);
        }
        else
        {
            return ServiceResponse<bool>.FailureResult($"Failed to send command: {response.StatusCode}");
        }
    }
    
}
