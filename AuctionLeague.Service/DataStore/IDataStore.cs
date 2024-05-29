namespace AuctionLeague.Service.DataStore
{
    public interface IDataStore<T>
    {
        T Data { get; set; }
    }
}