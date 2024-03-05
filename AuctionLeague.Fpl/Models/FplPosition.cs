namespace AuctionLeague.Fpl.Models;

public class FplPosition
{
  public int id { get; set; }
  public string plural_name_short { get; set; }
  public string singular_name { get; set; }
  public string singular_name_short { get; set; }
  public int squad_select { get; set; }
  public int squad_min_play { get; set; }
  public int squad_max_play { get; set; }
  public int element_count { get; set; }
}