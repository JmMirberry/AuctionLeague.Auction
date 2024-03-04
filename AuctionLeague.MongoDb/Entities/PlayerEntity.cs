namespace AuctionLeague.MongoDb.Entities
{
    public class PlayerEntity
    {
        public int PlayerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Team { get; set; }
        public string Position { get; set; }
        public int Value { get; set; }
        public int TotalPoints { get; set; }
    }
}