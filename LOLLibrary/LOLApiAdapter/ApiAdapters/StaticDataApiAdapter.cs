namespace LOLApiAdapter.ApiAdapters
{
    public class StaticDataApiAdapter : LoLApiAdapterBase
    {
        public static string GetChampionsStaticData(string region)
        {
            return $"{GetApiHost(region)}/lol/static-data/v3/champions";
        }
    }
}
