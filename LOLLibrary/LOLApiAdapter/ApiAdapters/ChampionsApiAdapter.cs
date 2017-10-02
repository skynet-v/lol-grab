namespace LOLApiAdapter.ApiAdapters
{
    public class ChampionsApiAdapter : LoLApiAdapterBase
    {
        public static string GetChampions(string region)
        {
            return $"{GetApiHost(region)}/lol/platform/v3/champions";
        }

        public static string GetChampionById(long id, string region)
        {
            return $"{GetApiHost(region)}/lol/platform/v3/champions{id}";
        }
    }
}
