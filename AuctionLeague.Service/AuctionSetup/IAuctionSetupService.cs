
using AuctionLeague.Data.Settings;

namespace AuctionLeague.Service
{
    public interface IAuctionSetupService
    {
        Task InitialiseAuctionData();
        Task SetAutoNomination(List<AutonominationSettings> settings);
        Task ResetSold();
    }
}