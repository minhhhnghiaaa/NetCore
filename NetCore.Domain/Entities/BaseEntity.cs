namespace NetCore.Domain.Entities;

public class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public int Status { get; set; }
}