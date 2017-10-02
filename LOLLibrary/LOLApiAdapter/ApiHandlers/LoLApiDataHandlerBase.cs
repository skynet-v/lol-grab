using LOLApiAdapter.Utils;

namespace LOLApiAdapter.ApiHandlers
{
    public abstract class LoLApiDataHandlerBase
    {
        public LoLWebAdapter Handler;

        public string ApiKey;
        public string Region;

        public LoLApiDataHandlerBase(string apiKey, string region)
        {
            ApiKey = apiKey;
            Region = region;

            Handler = new LoLWebAdapter();
        }

        protected string AppendApiKey(string query)
        {
            return $"{query}?api_key={ApiKey}";
        }
    }
}