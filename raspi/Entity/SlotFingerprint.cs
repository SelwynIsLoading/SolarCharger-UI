namespace raspi.Entity;

public class SlotFingerprint : AuditableEntity
{
    public int FingerprintId { get; set; }
    public string SlotNumber { get; set; } = "";
    public bool IsActive { get; set; }
    public int CoinTotal { get; set; }
}