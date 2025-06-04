namespace raspi.Services;

public class CountdownManager
{
    private readonly Dictionary<string, CountdownService> _slots = new();
    private readonly ArduinoService _arduinoService;

    public CountdownManager(ArduinoService arduinoService)
    {
        _arduinoService = arduinoService;
    }

    public CountdownService GetOrCreate(string slotId)
    {
        if (!_slots.ContainsKey(slotId))
        {
            var service = new CountdownService(_arduinoService);
            service.SlotNumber = slotId;
            _slots[slotId] = service;
        }

        return _slots[slotId];
    }
}