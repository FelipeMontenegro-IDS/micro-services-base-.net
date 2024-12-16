namespace Domain.Common;

public abstract class BaseEntity
{
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public DateTime? Deleted { get; set; }
}