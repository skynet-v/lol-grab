namespace LOLApiAdapter.ApiAdapters
{
    public class MatchApiAdapter : LoLApiAdapterBase
    {
        public string GetMatchByMatchIdRequestUrl(long matchId, string region)
        {
            return $"{GetApiHost(region)}/lol/match/v3/matches/{matchId}";
        }

        public string GetMatchByAccountIdRequestUrl(long accountId, string region)
        {
            return $"{GetApiHost(region)}/lol/match/v3/matchlists/by-account/{accountId}";
        }

        public string GetRecentMatchByAccountIdRequestUrl(long accountId, string region)
        {
            return $"{GetApiHost(region)}/lol/match/v3/matchlists/by-account/{accountId}/recent";
        }

        public string GetMatchTimelineByMatchIdRequestUrl(long matchId, string region)
        {
            return $"{GetApiHost(region)}/lol/match/v3/timelines/by-match/{matchId}";
        }

        public string GetMatchesByTournamentCodeRequestUrl(long tournamentCode, string region)
        {
            return $"{GetApiHost(region)}/lol/match/v3/matches/by-tournament-code/{tournamentCode}/ids";
        }

        public string GetMatchByMatchIdAndTournamentCodeRequestUrl(long matchId, long tournamentCode, string region)
        {
            return $"{GetApiHost(region)}/lol/match/v3/matches/{matchId}/by-tournament-code/{tournamentCode}";
        }
    }
}