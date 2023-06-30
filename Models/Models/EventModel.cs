namespace UP.Core.Models;

public class EventModel : BaseModel
{
    public bool IsLive { get; set; }

    public string CategoryId { get; set; }

    public int SportModelId { get; set; }
}