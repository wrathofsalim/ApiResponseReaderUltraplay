namespace UP.Core.Models;

public class SingleMatchModel : BaseModel
{
    public DateTime StartDate { get; set; }

    public string MatchType { get; set; }

    public List<BetModel>? BetModels { get; set; }
    
    public List<OddModel>? OddModels { get; set; }
}