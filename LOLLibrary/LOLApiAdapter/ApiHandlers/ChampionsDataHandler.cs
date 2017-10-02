using System.Threading.Tasks;
using LOLApiAdapter.ApiAdapters;
using LOLApiAdapter.ApiResponseStructures;
using LOLApiAdapter.CommonDefinitions.Interfaces;
using LOLApiAdapter.Utils;

namespace LOLApiAdapter.ApiHandlers
{
    public class ChampionsDataHandler : LoLApiDataHandlerBase
    {
        public ChampionsDataHandler(string apiKey, string region) : base(apiKey, region)
        {
        }

        public ILoLResponse GetChampions()
        {
            var queryUrl = ChampionsApiAdapter.GetChampions(Region);
            queryUrl = AppendApiKey(queryUrl);

            return WebUtilFunctions.GetEndpointResponseFromUrl<ChampionsResponse, ErrorResponse>(Handler, queryUrl);
        }

        public async Task<ILoLResponse> GetChampionsAsync()
        {
            var queryUrl = ChampionsApiAdapter.GetChampions(Region);
            queryUrl = AppendApiKey(queryUrl);

            return await WebUtilFunctions.GetEndpointResponseFromUrlAsync<ChampionsResponse, ErrorResponse>(Handler, queryUrl);
        }

        public ILoLResponse GetChampionById(long id)
        {
            var queryUrl = ChampionsApiAdapter.GetChampionById(id, Region);
            queryUrl = AppendApiKey(queryUrl);

            return WebUtilFunctions.GetEndpointResponseFromUrl<ChampionsResponse, ErrorResponse>(Handler, queryUrl);
        }

        public async Task<ILoLResponse> GetChampionByIdAsync(long id)
        {
            var queryUrl = ChampionsApiAdapter.GetChampionById(id, Region);
            queryUrl = AppendApiKey(queryUrl);

            return await WebUtilFunctions.GetEndpointResponseFromUrlAsync<ChampionsResponse, ErrorResponse>(Handler, queryUrl);
        }
    }
}
