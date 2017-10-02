using System;
using System.Threading.Tasks;
using LOLApiAdapter.ApiAdapters;
using LOLApiAdapter.ApiResponseStructures;
using LOLApiAdapter.CommonDefinitions.Interfaces;
using LOLApiAdapter.Utils;

namespace LOLApiAdapter.ApiHandlers
{
    public class MatchDetailsDataHandler : LoLApiDataHandlerBase
    {
        readonly MatchApiAdapter _adapter = new MatchApiAdapter();

        public MatchDetailsDataHandler(string apiKey, string region) : base(apiKey, region)
        {   
        }

        public ILoLResponse GetMatchByMatchId(long matchId)
        {
            var queryUrl = _adapter.GetMatchByMatchIdRequestUrl(matchId, Region);
            queryUrl = AppendApiKey(queryUrl);

            return WebUtilFunctions.GetEndpointResponseFromUrl<MatchDetailsResponse, ErrorResponse>(Handler, queryUrl);
        }

        public async Task<ILoLResponse> GetMatchByMatchIdAsync(long matchId)
        {
            var queryUrl = _adapter.GetMatchByMatchIdRequestUrl(matchId, Region);
            queryUrl = AppendApiKey(queryUrl);

            return await WebUtilFunctions.GetEndpointResponseFromUrlAsync<MatchDetailsResponse, ErrorResponse>(Handler, queryUrl);
        }

        public ILoLResponse GetMatchListByAccountId(long accountId)
        {
            var queryUrl = _adapter.GetMatchByAccountIdRequestUrl(accountId, Region);
            queryUrl = AppendApiKey(queryUrl);

            return WebUtilFunctions.GetEndpointResponseFromUrl<MatchListsResponse, ErrorResponse>(Handler, queryUrl);
        }

        public async Task<ILoLResponse> GetMatchListByAccountIdAsync(long accountId)
        {
            var queryUrl = _adapter.GetMatchByAccountIdRequestUrl(accountId, Region);
            queryUrl = AppendApiKey(queryUrl);

            return await WebUtilFunctions.GetEndpointResponseFromUrlAsync<MatchListsResponse, ErrorResponse>(Handler, queryUrl);
        }

        public ILoLResponse GetRecentMatchByAccountId(long accountId)
        {
            var queryUrl = _adapter.GetRecentMatchByAccountIdRequestUrl(accountId, Region);
            queryUrl = AppendApiKey(queryUrl);

            return WebUtilFunctions.GetEndpointResponseFromUrl<MatchListsResponse, ErrorResponse>(Handler, queryUrl);
        }

        public async Task<ILoLResponse> GetRecentMatchByAccountIdAsync(long accountId)
        {
            var queryUrl = _adapter.GetRecentMatchByAccountIdRequestUrl(accountId, Region);
            queryUrl = AppendApiKey(queryUrl);

            return await WebUtilFunctions.GetEndpointResponseFromUrlAsync<MatchListsResponse, ErrorResponse>(Handler, queryUrl);
        }

        public ILoLResponse GetMatchTimelineByMatchId(long matchId)
        {
            var queryUrl = _adapter.GetMatchTimelineByMatchIdRequestUrl(matchId, Region);
            queryUrl = AppendApiKey(queryUrl);

            return WebUtilFunctions.GetEndpointResponseFromUrl<MatchTimeLineResponse, ErrorResponse>(Handler, queryUrl);
        }

        public async Task<ILoLResponse> GetMatchTimelineByMatchIdAsync(long matchId)
        {
            var queryUrl = _adapter.GetMatchTimelineByMatchIdRequestUrl(matchId, Region);
            queryUrl = AppendApiKey(queryUrl);

            return await WebUtilFunctions.GetEndpointResponseFromUrlAsync<MatchTimeLineResponse, ErrorResponse>(Handler, queryUrl);
        }

        public ILoLResponse GetMatchesByTournamentCode(long tournamentCode)
        {
            var queryUrl = _adapter.GetMatchTimelineByMatchIdRequestUrl(tournamentCode, Region);
            queryUrl = AppendApiKey(queryUrl);

            return WebUtilFunctions.GetEndpointResponseFromUrl<MatchDetailsResponse, ErrorResponse>(Handler, queryUrl);
        }

        public async Task<ILoLResponse> GetMatchesByTournamentCodeAsync(long tournamentCode)
        {
            var queryUrl = _adapter.GetMatchTimelineByMatchIdRequestUrl(tournamentCode, Region);
            queryUrl = AppendApiKey(queryUrl);

            return await WebUtilFunctions.GetEndpointResponseFromUrlAsync<MatchDetailsResponse, ErrorResponse>(Handler, queryUrl);
        }

        public ILoLResponse GetMatchByMatchIdAndTournamentCode(long matchId, long tournamentCode)
        {
            var queryUrl = _adapter.GetMatchByMatchIdAndTournamentCodeRequestUrl(matchId, tournamentCode, Region);
            queryUrl = AppendApiKey(queryUrl);

            return WebUtilFunctions.GetEndpointResponseFromUrl<MatchDetailsResponse, ErrorResponse>(Handler, queryUrl);
        }

        public async Task<ILoLResponse> GetMatchByMatchIdAndTournamentCodeAsync(long matchId, long tournamentCode)
        {
            var queryUrl = _adapter.GetMatchByMatchIdAndTournamentCodeRequestUrl(matchId, tournamentCode, Region);
            queryUrl = AppendApiKey(queryUrl);

            return await WebUtilFunctions.GetEndpointResponseFromUrlAsync<MatchDetailsResponse, ErrorResponse>(Handler, queryUrl);
        }
    }
}
