using AuctionLeague.Data.FplPlayer;

namespace AuctionLeague.MongoDb;

public class ManualPlayerEntity
{
    public string _id {  get; set; }
    public int PlayerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}