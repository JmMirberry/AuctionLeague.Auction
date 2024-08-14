using AuctionLeague.Data.FplPlayer;

namespace AuctionLeague.Data.Settings;

public class AutonominationSettings
{
    public int Round { get; set; }
    public double GkpMinValue { get; set; }
    public double DefMinValue { get; set; }
    public double MidMinValue { get; set; }
    public double FwdMinValue { get; set; }
}