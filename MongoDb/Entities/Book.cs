using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuctionLeague.MongoDb.Models
{
    public class Book
    {
        [BsonElement("Name")]
        public string BookName { get; set; } = null!;

        public decimal Price { get; set; }

        public string Category { get; set; } = null!;

        public string Author { get; set; } = null!;
    }
}