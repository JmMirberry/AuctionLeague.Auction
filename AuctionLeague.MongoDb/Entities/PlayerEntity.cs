using AuctionLeague.Data.FplPlayer;

namespace AuctionLeague.MongoDb;

public class PlayerEntity
{
    public string _id {  get; set; }
    public int PlayerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Team { get; set; }
    public Position Position { get; set; }
    public double Value { get; set; }
    public int TotalPointsPreviousYear { get; set; }
}