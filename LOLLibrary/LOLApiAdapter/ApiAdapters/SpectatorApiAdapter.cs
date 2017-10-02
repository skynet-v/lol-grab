using LOLApiAdapter.ApiAdapters;

namespace LOLApiAdapter
{
    public class SpectatorApiAdapter : LoLApiAdapterBase
    {
        public static string GetActiveGamesBySummonerId(long summonerId, string region)
        {
            return $"{GetApiHost(region)}/lol/spectator/v3/active-games/by-summoner/{summonerId}";
        }

        public static string GetFeaturedGames(string region)
        {
            return $"{GetApiHost(region)}/lol/spectator/v3/featured-games";
        }
    }
}