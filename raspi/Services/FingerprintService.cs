using Microsoft.EntityFrameworkCore;
using raspi.DTOs;

namespace raspi.Services;

public class FingerprintService(ApplicationDbContext context, HttpClient client)
{
    public async Task<ServiceResponse<int>> EnrollAsync(string slotNumber)
    {
        // var model = await context.SlotFingerprints
        //     .Where(s => s.SlotNumber == slotNumber)
        //     .OrderByDescending(ob => ob.Created)
        //     .FirstOrDefaultAsync();
        //
        // if (model is null)
        // {
        //     return ServiceResponse<int>.FailureResult("Slot not found");
        // }
        //
        // if (model.IsActive)
        // {
        //     return ServiceResponse<int>.FailureResult("This slot has its owner's device in it. Please place the user's fingerprint to continue");
        // }
        
        var response = await client.PostAsync("/fingerprint/enroll", null);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<FingerprintEnrollResponse>();
            if(result is null)
                return ServiceResponse<int>.FailureResult("invalid response");
            return ServiceResponse<int>.SuccessResult(result.FingerprintId);
        }
        return ServiceResponse<int>.FailureResult("invalid response");
    }

    public async Task<ServiceResponse<bool>> DeleteFingerprint(int id)
    {
        var response = await client.PostAsJsonAsync("/fingerprint/delete", new {id});

        // var model = await context.SlotFingerprints
        //     .Where(w => w.FingerprintId == id && w.IsActive)
        //     .OrderByDescending(ob => ob.Created)
        //     .FirstOrDefaultAsync();
        //
        // if (model is null)
        //     return ServiceResponse<bool>.FailureResult("Slot not found");
        //
        // model.IsActive = false;
        // await context.SaveChangesAsync();
        
        return response.IsSuccessStatusCode
            ? ServiceResponse<bool>.SuccessResult(true)
            : ServiceResponse<bool>.FailureResult(response.Content.ReadAsStringAsync().Result);

    }

    public async Task<ServiceResponse<bool>> CheckIfStillActive(string slotNumber)
    {
        var model = await context.SlotFingerprints
            .AsNoTracking()
            .Where(w => w.SlotNumber == slotNumber)
            .OrderByDescending(ob => ob.Created)
            .FirstOrDefaultAsync();
        
        if(model is null)
            return ServiceResponse<bool>.FailureResult("Slot not found");
        
        return ServiceResponse<bool>.SuccessResult(model.IsActive);
    }
}

public class FingerprintEnrollResponse
{
    public int FingerprintId { get; set; }
}