using System.ComponentModel.DataAnnotations.Schema;

namespace UP.Core.Entities;

public class EventEntity : BaseEntity
{
    public bool IsLive { get; set; }

    public string CategoryId { get; set; }

    public ICollection<MatchEntity> Matches { get; set; } = new List<MatchEntity>();

    public int SportEntityId { get; set; }

    [ForeignKey("SportEntityId")]
    public SportEntity SportEntity { get; set; }
}