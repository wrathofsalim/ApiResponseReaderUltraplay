namespace UP.Core.Models;

public class BetModel : BaseModel
{
    public bool IsLive { get; set; }

    public int MatchModelId { get; set; }

    public List<OddModel>? OddModels { get; set; }
}