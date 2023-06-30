namespace UP.Core.Entities;

public class SportEntity : BaseEntity
{
    public ICollection<EventEntity> Events { get; set; } = new List<EventEntity>();
}