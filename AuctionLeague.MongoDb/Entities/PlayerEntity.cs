namespace AuctionLeague.MongoDb.Entities
{
    public class PlayerEntity
    {

        public int _id => PlayerId;
        public int PlayerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Team { get; set; }
        public string Position { get; set; }
        public double Value { get; set; }
        public int TotalPointsPreviousYear { get; set; }
        public bool IsInFpl { get; set; }
    }
}