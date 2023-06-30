using System.ComponentModel.DataAnnotations.Schema;

namespace UP.Core.Entities;

public class BetEntity : BaseEntity
{
    public bool IsLive { get; set; }

    public ICollection<OddEntity> Odds { get; set; } = new List<OddEntity>();

    public int MatchEntityId { get; set; }

    [ForeignKey("MatchEntityId")]
    public MatchEntity MatchEntity { get; set; }
}