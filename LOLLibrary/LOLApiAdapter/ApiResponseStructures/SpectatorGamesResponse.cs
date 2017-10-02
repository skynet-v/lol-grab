using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOLApiAdapter.CommonDefinitions;
using LOLApiAdapter.CommonDefinitions.Interfaces;

namespace LOLApiAdapter.ApiResponseStructures
{
    public class SpectatorGamesResponse : ILoLResponse
    {
        public int clientRefreshInterval { get; set; }
        public Gamelist[] gameList { get; set; }
    }

    public class Gamelist : ILoLResponse
    {
        public long gameId { get; set; }
        public long gameStartTime { get; set; }
        public string platformId { get; set; }
        public string gameMode { get; set; }
        public long mapId { get; set; }
        public string gameType { get; set; }
        public long gameQueueConfigId { get; set; }
        public Observers observers { get; set; }
        public FeaturedGameParticipant[] participants { get; set; }
        public long gameLength { get; set; }
        public object[] bannedChampions { get; set; }
    }

    public class Observers
    {
        public string encryptionKey { get; set; }
    }


    public class FeaturedGameParticipant
    {
        public int profileIconId { get; set; }
        public int championId { get; set; }
        public string summonerName { get; set; }
        public bool bot { get; set; }
        public int spell2Id { get; set; }
        public int teamId { get; set; }
        public int spell1Id { get; set; }
    }

}
