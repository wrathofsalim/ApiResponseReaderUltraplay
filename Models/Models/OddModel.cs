namespace UP.Core.Models;

public class OddModel : BaseModel
{
    public decimal Value { get; set; }

    public decimal? SpecialBetValue { get; set; }

    public int BetModelId { get; set; }
}