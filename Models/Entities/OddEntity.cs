using System.ComponentModel.DataAnnotations.Schema;

namespace UP.Core.Entities;

public class OddEntity : BaseEntity
{
    public decimal Value { get; set; }

    public decimal? SpecialBetValue { get; set; }

    public int BetEntityId { get; set; }

    [ForeignKey("BetEntityId")]
    public BetEntity BetEntity { get; set; }
}