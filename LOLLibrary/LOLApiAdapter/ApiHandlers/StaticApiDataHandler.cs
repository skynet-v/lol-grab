using System.Threading.Tasks;
using LOLApiAdapter.ApiAdapters;
using LOLApiAdapter.ApiResponseStructures;
using LOLApiAdapter.CommonDefinitions.Interfaces;
using LOLApiAdapter.Utils;

namespace LOLApiAdapter.ApiHandlers
{
    public class StaticApiDataHandler : LoLApiDataHandlerBase
    {
        public StaticApiDataHandler(string apiKey, string region) : base(apiKey, region)
        {
        }

        public ILoLResponse GetChampions()
        {
            var queryUrl = StaticDataApiAdapter.GetChampionsStaticData(Region);
            queryUrl = $"{AppendApiKey(queryUrl)}&locale=en_US&tags=all&dataById=false";

            return WebUtilFunctions.GetEndpointResponseFromUrl<StaticDataChampionsResponse, ErrorResponse>(Handler, queryUrl);
        }

        public async Task<ILoLResponse> GetChampionsAsync()
        {
            var queryUrl = StaticDataApiAdapter.GetChampionsStaticData(Region);
            queryUrl = $"{AppendApiKey(queryUrl)}&locale=en_US&tags=all&dataById=false";

            return await WebUtilFunctions.GetEndpointResponseFromUrlAsync<StaticDataChampionsResponse, ErrorResponse>(Handler, queryUrl);
        }
    }
}
