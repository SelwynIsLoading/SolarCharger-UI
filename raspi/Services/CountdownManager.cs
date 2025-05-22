namespace raspi.Services;

public class CountdownManager
{
    private readonly Dictionary<string, CountdownService> _slots = new();

    public CountdownService GetOrCreate(string slotId)
    {
        if (!_slots.ContainsKey(slotId))
            _slots[slotId] = new CountdownService();

        return _slots[slotId];
    }
}