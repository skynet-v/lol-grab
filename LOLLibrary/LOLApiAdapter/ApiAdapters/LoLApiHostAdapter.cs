using System;
using LOLApiAdapter.CommonDefinitions.Enums;

namespace LOLApiAdapter.ApiAdapters
{
    public class LoLApiAdapterBase
    {
        private static readonly string MainHostTemplate = "https://{0}.api.riotgames.com";
        protected static string GetApiHost(string region)
        {
            return string.Format(MainHostTemplate, region);
        }

        public static LoLApiAdapterBase GetAdapter(LoLApiType type)
        {
            switch (type)
            {
                case LoLApiType.SUMMONER_V3:
                    return new SummonerApiAdapter();
                case LoLApiType.MATCH_V3:
                    return new MatchApiAdapter();
                case LoLApiType.SPECTATOR_V3:
                    return new SpectatorApiAdapter();
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
