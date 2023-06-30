using System.ComponentModel.DataAnnotations.Schema;

namespace UP.Core.Entities;

public class MatchEntity : BaseEntity
{
    public DateTime StartDate { get; set; }

    public string MatchType { get; set; }

    public ICollection<BetEntity> Bets { get; set; } = new List<BetEntity>();

    public int EventEntityId { get; set; }

    [ForeignKey("EventEntityId")]
    public EventEntity EventEntity { get; set; }
}