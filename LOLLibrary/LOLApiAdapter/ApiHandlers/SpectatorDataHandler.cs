using System.Threading.Tasks;
using LOLApiAdapter.ApiResponseStructures;
using LOLApiAdapter.CommonDefinitions.Interfaces;
using LOLApiAdapter.Utils;

namespace LOLApiAdapter.ApiHandlers
{
    public class SpectatorDataHandler : LoLApiDataHandlerBase
    {
        public SpectatorDataHandler(string apiKey, string region) : base(apiKey, region)
        {
        }

        public ILoLResponse GetFeaturedGames()
        {
            var queryUrl = SpectatorApiAdapter.GetFeaturedGames(Region);
            queryUrl = AppendApiKey(queryUrl);

            return WebUtilFunctions.GetEndpointResponseFromUrl<SpectatorGamesResponse, ErrorResponse>(Handler, queryUrl);
        }
        public async Task<ILoLResponse> GetFeaturedGamesAsync()
        {
            var queryUrl = SpectatorApiAdapter.GetFeaturedGames(Region);
            queryUrl = AppendApiKey(queryUrl);

            return await WebUtilFunctions.GetEndpointResponseFromUrlAsync<SpectatorGamesResponse, ErrorResponse>(Handler, queryUrl);
        }

        public ILoLResponse GetActiveGamesBySummonerId(long summonerId)
        {
            var queryUrl = SpectatorApiAdapter.GetActiveGamesBySummonerId(summonerId, Region);
            queryUrl = AppendApiKey(queryUrl);

            return WebUtilFunctions.GetEndpointResponseFromUrl<Gamelist, ErrorResponse>(Handler, queryUrl);
        }

        public async Task<ILoLResponse> GetActiveGamesBySummonerIdAsync(long summonerId)
        {
            var queryUrl = SpectatorApiAdapter.GetActiveGamesBySummonerId(summonerId, Region);
            queryUrl = AppendApiKey(queryUrl);

            return await WebUtilFunctions.GetEndpointResponseFromUrlAsync<Gamelist, ErrorResponse>(Handler, queryUrl);
        }
    }
}
