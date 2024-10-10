namespace signa.Entities;

public class BaseEntity(Guid id, DateTime createdAt)
{
    public Guid Id { get; set; } = id;

    public DateTime CreatedAt { get; set; } = createdAt;

    public DateTime UpdatedAt { get; set; } = createdAt;
}