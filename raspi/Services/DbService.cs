using Microsoft.EntityFrameworkCore;
using raspi.DTOs;
using raspi.Entity;

namespace raspi.Services;

public class DbService(ApplicationDbContext db)
{
    private readonly ApplicationDbContext _db = db;

    public async Task<ServiceResponse<bool>> AddCoin(string slotNumber, int coinTotal, int fingerprintId)
    {
        var slot = new SlotFingerprint
        {
            Id = Guid.NewGuid(),
            SlotNumber = slotNumber,
            CoinTotal = coinTotal,
            FingerprintId = fingerprintId,
            IsActive = true,
            Created = DateTime.UtcNow
        };
        await _db.SlotFingerprints.AddAsync(slot);
        await _db.SaveChangesAsync();
        return new ServiceResponse<bool>(true);
    }

    public async Task<ServiceResponse<SlotFingerprint?>> GetActiveSlotInfoAsync(string slotNumber)
    {
        var slot = await _db.SlotFingerprints
            .Where(w => w.SlotNumber == slotNumber && w.IsActive)
            .OrderByDescending(ob => ob.Created)
            .FirstOrDefaultAsync();
        
        return new ServiceResponse<SlotFingerprint?>(slot);
    }

    public async Task<ServiceResponse<bool>> MarkAsInActiveAsync(string slotNumber)
    {
        var slots = await _db.SlotFingerprints
            .Where(w => w.SlotNumber == slotNumber && w.IsActive)
            .OrderByDescending(ob => ob.Created)
            .ToListAsync();

        foreach (var slot in slots)
        {
            slot.IsActive = false;
        }
        
        await _db.SaveChangesAsync();
        return new ServiceResponse<bool>(true);
    }

    public async Task<ServiceResponse<List<SlotFingerprint>>> GetReport(TimeFrame timeFrame)
    {
        var now = DateTime.UtcNow;
        var startDate = timeFrame switch
        {
            TimeFrame.Daily => now.Date,
            TimeFrame.Weekly => now.AddDays(-7),
            TimeFrame.Monthly => now.AddMonths(-1),
            _ => now.Date
        };

        var report = await _db.SlotFingerprints
            .Where(w => w.Created >= startDate && w.Created <= now)
            .OrderByDescending(o => o.Created)
            .ToListAsync();

        return new ServiceResponse<List<SlotFingerprint>>(report);
    }

    public enum TimeFrame
    {
        Daily,
        Weekly,
        Monthly
    }
}