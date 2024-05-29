namespace AuctionLeague.Service.DataStore
{
    public class DataStore<T> : IDataStore<T>
    {
        private readonly object _lock = new object();
        private T _data;

        public T Data
        {
            get
            {
                lock (_lock)
                {
                    return _data;
                }
            }
            set
            {
                lock (_lock)
                {
                    _data = value;
                }
            }
        }
    }
}
