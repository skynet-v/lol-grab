using System.Threading.Tasks;
using LOLApiAdapter.ApiAdapters;
using LOLApiAdapter.ApiResponseStructures;
using LOLApiAdapter.CommonDefinitions;
using LOLApiAdapter.CommonDefinitions.Interfaces;
using LOLApiAdapter.Utils;

namespace LOLApiAdapter.ApiHandlers
{
    public class SummonerDataHandler : LoLApiDataHandlerBase
    {
        public SummonerDataHandler(string apiKey, string region) : base(apiKey, region)
        {
        }

        public ILoLResponse GetSummonerByAccountId(long accountId)
        {
            var queryUrl = SummonerApiAdapter.GetSummonerByAccountIdRequestUrl(accountId, Region);
            queryUrl = AppendApiKey(queryUrl);

            return WebUtilFunctions.GetEndpointResponseFromUrl<SummonerResponse, ErrorResponse>(Handler, queryUrl);
        }

        public async Task<ILoLResponse> GetSummonerByAccountIdAsync(long accountId)
        {
            var queryUrl = SummonerApiAdapter.GetSummonerByAccountIdRequestUrl(accountId, Region);
            queryUrl = AppendApiKey(queryUrl);

            return await WebUtilFunctions.GetEndpointResponseFromUrlAsync<SummonerResponse, ErrorResponse>(Handler, queryUrl);
        }

        public ILoLResponse GetSummonerByName(string summonerName)
        {
            var queryUrl = SummonerApiAdapter.GetSummonerByNameRequestUrl(summonerName, Region);
            queryUrl = AppendApiKey(queryUrl);

            return WebUtilFunctions.GetEndpointResponseFromUrl<SummonerResponse, ErrorResponse>(Handler, queryUrl);
        }

        public async Task<ILoLResponse> GetSummonerByNameAsync(string summonerName)
        {
            var queryUrl = SummonerApiAdapter.GetSummonerByNameRequestUrl(summonerName, Region);
            queryUrl = AppendApiKey(queryUrl);

            return await WebUtilFunctions.GetEndpointResponseFromUrlAsync<SummonerResponse, ErrorResponse>(Handler, queryUrl);
        }

        public ILoLResponse GetSummonerBySummonerId(long summonerId)
        {
            var queryUrl = SummonerApiAdapter.GetSummonerBySummonerIdRequestUrl(summonerId, Region);
            queryUrl = AppendApiKey(queryUrl);

            return WebUtilFunctions.GetEndpointResponseFromUrl<SummonerResponse, ErrorResponse>(Handler, queryUrl);
        }

        public async Task<ILoLResponse> GetSummonerBySummonerIdAsync(long summonerId)
        {
            var queryUrl = SummonerApiAdapter.GetSummonerBySummonerIdRequestUrl(summonerId, Region);
            queryUrl = AppendApiKey(queryUrl);

            return await WebUtilFunctions.GetEndpointResponseFromUrlAsync<SummonerResponse, ErrorResponse>(Handler, queryUrl);
        }
    }
}