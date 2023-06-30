namespace UP.Core.Models;

public class MatchModel : BaseModel
{
    public DateTime StartDate { get; set; }

    public string MatchType { get; set; }

    public int EventModelId { get; set; }

    public List<BetModel>? BetModels { get; set; }
}