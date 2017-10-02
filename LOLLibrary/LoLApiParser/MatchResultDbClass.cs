using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace LoLApiParser
{
    public class MatchResultDbClass
    {
        public long     Id                                  { get; set; }
        public string   GameType                            { get; set; }
        public int      Winner                              { get; set; }
        public TimeSpan Duration                            { get; set; }
        public int      FirstBlood                          { get; set; }
        public int      FirstTower                          { get; set; }
        public int      FirstInhibitor                      { get; set; }
        public int      FirstDragon                         { get; set; }
        public int      FirstRiftHeraId                     { get; set; }
        public int      FirstBaron                          { get; set; }

        public List<ParticipantStatsEntry> ParticipantStats = new List<ParticipantStatsEntry>();
        public List<MatchTimeLineEntry> MatchLog = new List<MatchTimeLineEntry>();

        public static void UpdateMatchStatus(long matchId, DbAdapterBase Adapter, MySqlTransaction transaction, int status = 3)
        {
            var q = $"UPDATE MatchDescription md SET md.parse_status = {status}, md.dt_parsed = UTC_TIMESTAMP() WHERE md.id = {matchId};";
            using (var mlCmd = new MySqlCommand(q, Adapter.Connection))
            {
                if (transaction != null)
                    mlCmd.Transaction = transaction;

                try
                {
                    mlCmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    LoggersAdapter.Info($"Ошибка при обновлении статуса матча {matchId}: {e}");
                }
            }
        }
    }

    public class ParticipantStatsEntry
    {
        public int      ParticipantId                       { get; set; }
        public int      ChampionId                          { get; set; }
        public int      TeamId                              { get; set; }
        public bool     Bot                                 { get; set; }
        public int      Spell1                              { get; set; }
        public int      Spell2                              { get; set; }
        public string   Role                                { get; set; }
        public string   Lane                                { get; set; }
        public int      Kills                               { get; set; }
        public int      Deaths                              { get; set; }
        public int      Assists                             { get; set; }
        public int      TotalMinionsKilled                  { get; set; }
        public int      TotalDamageDealt                    { get; set; }
        public int      TotalDamageDealtToChampions         { get; set; }
        public int      MagicDamageDealt                    { get; set; }
        public int      MagicDamageDealtToChampions         { get; set; }
        public int      PhysicalDamageDealt                 { get; set; }
        public int      PhysicalDamageDealtToChampions      { get; set; }
        public int      GoldEarned                          { get; set; }
        public int      GoldSpent                           { get; set; }
        public int      ChampLevel                          { get; set; }
        public int      WardsPlaced                         { get; set; }
        public int      WardsKilled                         { get; set; }
    }

    public class MatchTimeLineEntry : ICloneable
    {
        public TimeSpan timestamp                           { get; set; }
        public int[]    gold                                { get; set; }
        public int[]    kills                               { get; set; }
        public int[]    assists                             { get; set; }
        public int[]    deaths                              { get; set; }
        
        public int      UPRIGHT_TurretsStatus               { get; set; }
        public int      DWNLEFT_TurretsStatus               { get; set; }
        public int      UPRIGHT_InhibitorsStatus            { get; set; }
        public int      DWNLEFT_InhibitorsStatus            { get; set; }
        
        //// TOP
        //public bool UPRIGHT_TopOuterTurretStatus            { get; set; }
        //public bool UPRIGHT_TopInnerTurretStatus            { get; set; }
        //public bool UPRIGHT_TopNexusTurretStatus            { get; set; }
        //public bool UPRIGHT_TopBaseTurretStatus             { get; set; }

        //public bool UPRIGHT_TopInhibitorStatus              { get; set; }

        //public bool DWNLEFT_TopOuterTurretStatus            { get; set; }
        //public bool DWNLEFT_TopInnerTurretStatus            { get; set; }
        //public bool DWNLEFT_TopNexusTurretStatus            { get; set; }
        //public bool DWNLEFT_TopBaseTurretStatus             { get; set; }

        //public bool DWNLEFT_TopInhibitorStatus              { get; set; }

        //// MID
        //public bool UPRIGHT_MidOuterTurretStatus            { get; set; }
        //public bool UPRIGHT_MidInnerTurretStatus            { get; set; }
        //public bool UPRIGHT_MidNexusTurretStatus            { get; set; }

        //public bool UPRIGHT_MidInhibitorStatus              { get; set; }

        //public bool DWNLEFT_MidOuterTurretStatus            { get; set; }
        //public bool DWNLEFT_MidInnerTurretStatus            { get; set; }
        //public bool DWNLEFT_MidNexusTurretStatus            { get; set; }

        //public bool DWNLEFT_MidInhibitorStatus              { get; set; }

        //// BOTTOM
        //public bool UPRIGHT_BotOuterTurretStatus            { get; set; }
        //public bool UPRIGHT_BotInnerTurretStatus            { get; set; }
        //public bool UPRIGHT_BotNexusTurretStatus            { get; set; }
        //public bool UPRIGHT_BotBaseTurretStatus             { get; set; }

        //public bool UPRIGHT_BotInhibitorStatus              { get; set; }

        //public bool DWNLEFT_BotOuterTurretStatus            { get; set; }
        //public bool DWNLEFT_BotInnerTurretStatus            { get; set; }
        //public bool DWNLEFT_BotNexusTurretStatus            { get; set; }
        //public bool DWNLEFT_BotBaseTurretStatus             { get; set; }

        //public bool DWNLEFT_BotInhibitorStatus              { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    public class BuildingManager
    {
        [Flags]
        public enum TurretType
        {
            OUTER_TOP = 1,
            INNER_TOP = 2,
            BASE_TOP = 4,
            OUTER_MID = 8,
            INNER_MID = 16,
            BASE_MID = 32,
            OUTER_BOT = 64,
            INNER_BOT = 128,
            BASE_BOT = 256,
            NEXUS_TOP = 512,
            NEXUS_BOT = 1024
        }

        [Flags]
        public enum InhibitorType
        {
            TOP_INHIBITOR = 1,
            MID_INHIBITOR = 2,
            BOT_INHIBITOR = 3,
        }

        public static TurretType GetTowerByLaneAndName(string lane, string towerName, bool wasNexusDestroyedBefore)
        {
            switch (lane)
            {
                case "MID_LANE":

                    switch (towerName)
                    {
                        case "OUTER_TURRET":
                            return TurretType.OUTER_MID;
                        case "INNER_TURRET":
                            return TurretType.INNER_MID;
                        case "BASE_TURRET":
                            return TurretType.BASE_MID;
                        case "NEXUS_TURRET":
                            return wasNexusDestroyedBefore ? TurretType.NEXUS_TOP : TurretType.NEXUS_BOT;
                        default: throw new ArgumentOutOfRangeException(nameof(towerName), towerName);
                    }

                case "TOP_LANE":
                    switch (towerName)
                    {
                        case "OUTER_TURRET":
                            return TurretType.OUTER_TOP;
                        case "INNER_TURRET":
                            return TurretType.INNER_TOP;
                        //case "NEXUS_TURRET":
                        //    return TurretType.NEXUS_TOP;
                        case "BASE_TURRET":
                            return TurretType.BASE_TOP;

                        default: throw new ArgumentOutOfRangeException(nameof(towerName), towerName);
                    }

                case "BOT_LANE":
                    switch (towerName)
                    {
                        case "OUTER_TURRET":
                            return TurretType.OUTER_BOT;
                        case "INNER_TURRET":
                            return TurretType.INNER_BOT;
                        //case "NEXUS_TURRET":
                        //    return TurretType.NEXUS_BOT;
                        case "BASE_TURRET":
                            return TurretType.BASE_BOT;
                        default: throw new ArgumentOutOfRangeException(nameof(towerName), towerName);
                    }

                default: throw new ArgumentOutOfRangeException(nameof(lane), lane);
            }
        }

        public static int KillTower(string towerName, string laneName, int currentStatus, bool wasNexusDestroyedBefore)
        {
            // зануляем бит
            return currentStatus & ~(int) GetTowerByLaneAndName(laneName, towerName, wasNexusDestroyedBefore);
        }

        public static InhibitorType GetInhibitorByLane(string lane)
        {
            switch (lane)
            {
                case "TOP_LANE":
                    return InhibitorType.TOP_INHIBITOR;
                case "MID_LANE":
                    return InhibitorType.MID_INHIBITOR;
                case "BOT_LANE":
                    return InhibitorType.BOT_INHIBITOR;
                default: throw new ArgumentOutOfRangeException(nameof(lane), lane);
            }
        }

        public static int KillInhibitor(string laneName, int currentStatus)
        {
            return currentStatus & ~(int) GetInhibitorByLane(laneName);
        }
    }
}
