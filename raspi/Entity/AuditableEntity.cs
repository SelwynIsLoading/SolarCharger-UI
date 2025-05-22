namespace raspi.Entity;

public abstract class AuditableEntity
{
    public Guid Id { get; set; }
    public DateTime Created { get; set; }
}