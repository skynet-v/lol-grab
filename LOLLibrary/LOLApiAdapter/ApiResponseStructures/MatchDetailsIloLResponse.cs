using System.Collections.Generic;
using LOLApiAdapter.CommonDefinitions.Interfaces;

namespace LOLApiAdapter.ApiResponseStructures
{
    public class MatchDetailsResponse : ILoLResponse
    {
        public long gameId { get; set; }
        public string platformId { get; set; }
        public long gameCreation { get; set; }
        public long gameDuration { get; set; }
        public long queueId { get; set; }
        public long mapId { get; set; }
        public long seasonId { get; set; }
        public string gameVersion { get; set; }
        public string gameMode { get; set; }
        public string gameType { get; set; }
        public Team[] teams { get; set; }
        public Participant[] participants { get; set; }
        public Participantidentity[] participantIdentities { get; set; }
    }

    public class Team
    {
        public int teamId { get; set; }
        public string win { get; set; }
        public bool firstBlood { get; set; }
        public bool firstTower { get; set; }
        public bool firstInhibitor { get; set; }
        public bool firstBaron { get; set; }
        public bool firstDragon { get; set; }
        public bool firstRiftHerald { get; set; }
        public int towerKills { get; set; }
        public int inhibitorKills { get; set; }
        public int baronKills { get; set; }
        public int dragonKills { get; set; }
        public int vilemawKills { get; set; }
        public int riftHeraldKills { get; set; }
        public int dominionVictoryScore { get; set; }
        public Ban[] bans { get; set; }
    }

    public class Ban
    {
        public int championId { get; set; }
        public int pickTurn { get; set; }
    }

    public class Participant
    {
        public int participantId { get; set; }
        public int teamId { get; set; }
        public int championId { get; set; }
        public int spell1Id { get; set; }
        public int spell2Id { get; set; }
        public Mastery[] masteries { get; set; }
        public Rune[] runes { get; set; }
        public string highestAchievedSeasonTier { get; set; }
        public Stats stats { get; set; }
        public Timeline timeline { get; set; }
    }

    public class Stats
    {
        public int participantId { get; set; }
        public bool win { get; set; }
        public int item0 { get; set; }
        public int item1 { get; set; }
        public int item2 { get; set; }
        public int item3 { get; set; }
        public int item4 { get; set; }
        public int item5 { get; set; }
        public int item6 { get; set; }
        public int kills { get; set; }
        public int deaths { get; set; }
        public int assists { get; set; }
        public int largestKillingSpree { get; set; }
        public int largestMultiKill { get; set; }
        public int killingSprees { get; set; }
        public int longestTimeSpentLiving { get; set; }
        public int doubleKills { get; set; }
        public int tripleKills { get; set; }
        public int quadraKills { get; set; }
        public int pentaKills { get; set; }
        public int unrealKills { get; set; }
        public int totalDamageDealt { get; set; }
        public int magicDamageDealt { get; set; }
        public int physicalDamageDealt { get; set; }
        public int trueDamageDealt { get; set; }
        public int largestCriticalStrike { get; set; }
        public int totalDamageDealtToChampions { get; set; }
        public int magicDamageDealtToChampions { get; set; }
        public int physicalDamageDealtToChampions { get; set; }
        public int trueDamageDealtToChampions { get; set; }
        public int totalHeal { get; set; }
        public int totalUnitsHealed { get; set; }
        public int damageSelfMitigated { get; set; }
        public int damageDealtToObjectives { get; set; }
        public int damageDealtToTurrets { get; set; }
        public int visionScore { get; set; }
        public int timeCCingOthers { get; set; }
        public int totalDamageTaken { get; set; }
        public int magicalDamageTaken { get; set; }
        public int physicalDamageTaken { get; set; }
        public int trueDamageTaken { get; set; }
        public int goldEarned { get; set; }
        public int goldSpent { get; set; }
        public int turretKills { get; set; }
        public int inhibitorKills { get; set; }
        public int totalMinionsKilled { get; set; }
        public int neutralMinionsKilled { get; set; }
        public int neutralMinionsKilledTeamJungle { get; set; }
        public int neutralMinionsKilledEnemyJungle { get; set; }
        public int totalTimeCrowdControlDealt { get; set; }
        public int champLevel { get; set; }
        public int visionWardsBoughtInGame { get; set; }
        public int sightWardsBoughtInGame { get; set; }
        public int wardsPlaced { get; set; }
        public int wardsKilled { get; set; }
        public bool firstBloodKill { get; set; }
        public bool firstBloodAssist { get; set; }
        public bool firstTowerKill { get; set; }
        public bool firstTowerAssist { get; set; }
        public bool firstInhibitorKill { get; set; }
        public bool firstInhibitorAssist { get; set; }
        public int combatPlayerScore { get; set; }
        public int objectivePlayerScore { get; set; }
        public int totalPlayerScore { get; set; }
        public int totalScoreRank { get; set; }
    }

    public class Timeline
    {
        public int participantId { get; set; }
        public Creepspermindeltas creepsPerMinDeltas { get; set; }
        public Xppermindeltas xpPerMinDeltas { get; set; }
        public Goldpermindeltas goldPerMinDeltas { get; set; }
        public Csdiffpermindeltas csDiffPerMinDeltas { get; set; }
        public Xpdiffpermindeltas xpDiffPerMinDeltas { get; set; }
        public Damagetakenpermindeltas damageTakenPerMinDeltas { get; set; }
        public Damagetakendiffpermindeltas damageTakenDiffPerMinDeltas { get; set; }
        public string role { get; set; }
        public string lane { get; set; }
    }

    public class Creepspermindeltas : Dictionary<string, double>
    {
    }

    public class Xppermindeltas : Dictionary<string, double>
    {
    }

    public class Goldpermindeltas : Dictionary<string, double>
    {
    }

    public class Csdiffpermindeltas : Dictionary<string, double>
    {
    }

    public class Xpdiffpermindeltas : Dictionary<string, double>
    {
    }

    public class Damagetakenpermindeltas : Dictionary<string, double>
    {
    }

    public class Damagetakendiffpermindeltas : Dictionary<string, double>
    {
    }

    public class Mastery
    {
        public int masteryId { get; set; }
        public int rank { get; set; }
    }

    public class Rune
    {
        public int runeId { get; set; }
        public int rank { get; set; }
    }

    public class Participantidentity
    {
        public int participantId { get; set; }
        public Player player { get; set; }
    }

    public class Player
    {
        public string platformId { get; set; }
        public int accountId { get; set; }
        public string summonerName { get; set; }
        public int summonerId { get; set; }
        public string currentPlatformId { get; set; }
        public int currentAccountId { get; set; }
        public string matchHistoryUri { get; set; }
        public int profileIcon { get; set; }
    }

    public class MatchListsResponse : ILoLResponse
    {
        public Match[] matches { get; set; }
        public int endIndex { get; set; }
        public int startIndex { get; set; }
        public int totalGames { get; set; }
    }

    public class Match
    {
        public string lane { get; set; }
        public int gameId { get; set; }
        public int champion { get; set; }
        public string platformId { get; set; }
        public long timestamp { get; set; }
        public int queue { get; set; }
        public string role { get; set; }
        public int season { get; set; }
    }

    public class MatchTimeLineResponse : ILoLResponse
    {
        public Frame[] frames { get; set; }
        public long frameInterval { get; set; }
    }

    public class Frame
    {
        public Participantframes participantFrames { get; set; }
        public Event[] events { get; set; }
        public long timestamp { get; set; }
    }

    public class Participantframes : Dictionary<int, ParticipantFrameDescription>
    {
    }

    public class ParticipantFrameDescription
    {
        public int participantId { get; set; }
        public Position position { get; set; }
        public int currentGold { get; set; }
        public int totalGold { get; set; }
        public int level { get; set; }
        public int xp { get; set; }
        public int minionsKilled { get; set; }
        public int jungleMinionsKilled { get; set; }
        public int dominionScore { get; set; }
        public int teamScore { get; set; }
    }

    public class Position
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    public class Event
    {
        // ВАЖНО!!!!!!!!
        public static string[] TimeLineEventTypes = new[]
        {
            "CHAMPION_KILL",
            "WARD_PLACED",
            "WARD_KILL",
            "BUILDING_KILL",
            "ELITE_MONSTER_KILL",
            "ITEM_PURCHASED",
            "ITEM_SOLD",
            "ITEM_DESTROYED",
            "ITEM_UNDO",
            "SKILL_LEVEL_UP",
            "ASCENDED_EVENT",
            "CAPTURE_POINT",
            "PORO_KING_SUMMON"
        };

        public string type { get; set; }
        public int timestamp { get; set; }
        public int participantId { get; set; }
        public int itemId { get; set; }
        public int skillSlot { get; set; }
        public string levelUpType { get; set; }
        public int afterId { get; set; }
        public int beforeId { get; set; }
        public Position position { get; set; }
        public int killerId { get; set; }
        public int victimId { get; set; }
        public int[] assistingParticipantIds { get; set; }
        public int teamId { get; set; }
        public string buildingType { get; set; }
        public string laneType { get; set; }
        public string towerType { get; set; }
    }
}