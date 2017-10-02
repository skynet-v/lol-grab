namespace LOLApiAdapter.ApiAdapters
{
    public class ApiStatusAdapter : LoLApiAdapterBase
    {
        public static string GetApiStatus(string region)
        {
            return $"{GetApiHost(region)}/lol/status/v3/shard-data";
        }
    }
}
