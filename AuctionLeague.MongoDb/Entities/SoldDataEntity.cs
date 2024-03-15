
namespace AuctionLeague.MongoDb
{
    public class SoldDataEntity
    {
        public string _id {  get; set; }
        public int PlayerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double SalePrice { get; set; }
        public string SoldTo { get; set; }

    }
}