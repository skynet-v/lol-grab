namespace LOLApiAdapter.ApiAdapters
{
    public class SummonerApiAdapter : LoLApiAdapterBase
    {
        public static string GetSummonerByAccountIdRequestUrl(long accountId, string region)
        {
            return $"{GetApiHost(region)}/lol/summoner/v3/summoners/by-account/{accountId}";
        }

        public static string GetSummonerByNameRequestUrl(string summonerName, string region)
        {
            return $"{GetApiHost(region)}/lol/summoner/v3/summoners/by-name/{summonerName}";
        }

        public static string GetSummonerBySummonerIdRequestUrl(long summonerId, string region)
        {
            return $"{GetApiHost(region)}/lol/summoner/v3/summoners/{summonerId}";
        }
    }
}