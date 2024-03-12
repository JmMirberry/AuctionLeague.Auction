namespace AuctionLeague.Data.Exceptions
{

    public class PlayerSaleException : Exception
    {
        public PlayerSaleException()
        {
        }

        public PlayerSaleException(string message)
            : base(message)
        {
        }

        public PlayerSaleException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}