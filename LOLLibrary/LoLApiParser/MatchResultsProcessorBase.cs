using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOLApiAdapter.ApiResponseStructures;
using MySql.Data.MySqlClient;

namespace LoLApiParser
{
    public interface IResultSaver
    {
    }

    public class ClassicMatchResultsProcessor : IResultSaver
    {
        public bool ProcessClassicMatchResultsResponse(MatchDetailsResponse response, MatchTimeLineResponse timelineResponse, DbAdapterBase Adapter)
        {
            if (response == null)
            {
                LoggersAdapter.Warn($"Пришел пустой {nameof(response)}");
                throw new ArgumentNullException(nameof(response));
            }

            if (timelineResponse == null)
            {
                LoggersAdapter.Warn($"Пришел пустой {nameof(timelineResponse)}");
                throw new ArgumentNullException(nameof(timelineResponse));
            }

            var teamDlStats = response.teams[0];
            var teamUrStats = response.teams[1];

            var matchResult = new MatchResultDbClass
            {
                Duration = TimeSpan.FromSeconds(response.gameDuration),
                GameType = response.gameType,
                Id = response.gameId,
            };

            FillCommonResultData(teamDlStats, teamUrStats, matchResult);
            FillMatchLogData(response, timelineResponse, matchResult);

            if (SaveMatchInfo(matchResult, Adapter))
                return true;

            LoggersAdapter.Info($"Не удалось сохранить информацию о матче в БД. Матч {response.gameId}");
            return false;
            
        }

        private bool SaveMatchInfo(MatchResultDbClass matchResult, DbAdapterBase Adapter)
        {
            #region MatchDescription Query
            var mdQuery = $"UPDATE MatchDescription md SET " +
                          $"md.game_type = \"{matchResult.GameType}\", " +
                          $"md.dt_parsed = UTC_TIMESTAMP(), " +
                          $"md.winner = {matchResult.Winner}, " +
                          $"md.duration = {(int)matchResult.Duration.TotalSeconds}," +
                          $"md.first_blood = {matchResult.FirstBlood}, " +
                          $"md.first_tower = {matchResult.FirstTower}, " +
                          $"md.fisrst_inhibitor = {matchResult.FirstInhibitor}," +
                          $"md.first_baron = {matchResult.FirstBaron}," +
                          $"md.first_dragon = {matchResult.FirstDragon}, " +
                          $"md.first_rift_hera_id = {matchResult.FirstRiftHeraId}" +
                          $" WHERE md.id = {matchResult.Id}";
            #endregion

            var partisipantsSb = new StringBuilder();
            foreach (var pStat in matchResult.ParticipantStats)
            {
                partisipantsSb.Append($"({matchResult.Id}, {pStat.ParticipantId}, {pStat.ChampionId}, {pStat.TeamId}, " +
                                      $"{Convert.ToInt32(pStat.Bot)}, {pStat.Spell1}, {pStat.Spell2}, \"{pStat.Role}\", \"{pStat.Lane}\", " +
                                      $"{pStat.Kills}, {pStat.Deaths}, {pStat.Assists}, " +
                                      $"{pStat.TotalMinionsKilled}, {pStat.TotalDamageDealt}, {pStat.MagicDamageDealt}, {pStat.PhysicalDamageDealt}, " +
                                      $"{pStat.TotalDamageDealtToChampions}, {pStat.MagicDamageDealtToChampions}, {pStat.PhysicalDamageDealtToChampions}, " +
                                      $"{pStat.GoldEarned}, {pStat.GoldSpent}, {pStat.ChampLevel}, {pStat.WardsPlaced}, {pStat.WardsKilled}),");
            }
            partisipantsSb.Length--; // убираем последнюю запятую

            #region ParticipantHeroesMatchStats Query
            var pStatQuery = $@"INSERT INTO ParticipantHeroesMatchStats (id_match, participant_id, champion_id, team_id, bot, spell1_id, spell2_id, role, lane, 
            kills, deaths, assists, 
            total_minions_killed, total_damage_dealt, magic_damage_dealt, physical_damage_dealt, 
            total_damage_dealt_to_champions, magic_damage_dealt_to_champions, physical_damage_dealt_to_champions, 
            gold_earned, gold_spent, champ_level, wards_placed, wards_killed)
            VALUES {partisipantsSb}
            ON DUPLICATE KEY UPDATE
                champion_id = VALUES(champion_id),
                team_id = VALUES(team_id),
                spell1_id = VALUES(spell1_id),
                spell2_id = VALUES(spell2_id),
                role = VALUES(role),
                lane = VALUES(lane),
                kills = VALUES(kills),
                deaths = VALUES(deaths),
                assists = VALUES(assists),
                total_minions_killed = VALUES(total_minions_killed),
                total_damage_dealt = VALUES(total_damage_dealt),
                magic_damage_dealt = VALUES(magic_damage_dealt),
                physical_damage_dealt = VALUES(physical_damage_dealt),
                total_damage_dealt_to_champions = VALUES(total_damage_dealt_to_champions),
                magic_damage_dealt_to_champions = VALUES(magic_damage_dealt_to_champions),
                physical_damage_dealt_to_champions = VALUES(physical_damage_dealt_to_champions),
                gold_earned = VALUES(gold_earned),
                gold_spent = VALUES(gold_spent),
                champ_level = VALUES(champ_level),
                wards_placed = VALUES(wards_placed),
                wards_killed = VALUES(wards_killed)";
            #endregion

            var mlSb = new StringBuilder();
            foreach (var entry in matchResult.MatchLog)
            {
                mlSb.Append($"({matchResult.Id}, {(int)entry.timestamp.TotalSeconds}, " +
                            $"{entry.gold[0]},{entry.gold[1]},{entry.gold[2]},{entry.gold[3]},{entry.gold[4]},{entry.gold[5]},{entry.gold[6]},{entry.gold[7]},{entry.gold[8]},{entry.gold[9]}, " +
                            $"{entry.kills[0]},{entry.kills[1]},{entry.kills[2]},{entry.kills[3]},{entry.kills[4]},{entry.kills[5]},{entry.kills[6]},{entry.kills[7]},{entry.kills[8]},{entry.kills[9]}, " +
                            $"{entry.assists[0]},{entry.assists[1]},{entry.assists[2]},{entry.assists[3]},{entry.assists[4]},{entry.assists[5]},{entry.assists[6]},{entry.assists[7]},{entry.assists[8]},{entry.assists[9]}, " +
                            $"{entry.deaths[0]},{entry.deaths[1]},{entry.deaths[2]},{entry.deaths[3]},{entry.deaths[4]},{entry.deaths[5]},{entry.deaths[6]},{entry.deaths[7]},{entry.deaths[8]},{entry.deaths[9]}, " +
                            $"{entry.DWNLEFT_TurretsStatus},{entry.DWNLEFT_InhibitorsStatus}, " +
                            $"{entry.UPRIGHT_TurretsStatus},{entry.UPRIGHT_InhibitorsStatus}),");
            }
            mlSb.Length--;

            #region MatchLog Query
            var mlQuery = $@"INSERT INTO MatchLog (id_match, time,
            gold1, gold2, gold3, gold4, gold5, gold6, gold7, gold8, gold9, gold10, 
            kills1, kills2, kills3, kills4, kills5, kills6, kills7, kills8, kills9, kills10, 
            assists1, assists2, assists3, assists4, assists5, assists6, assists7, assists8, assists9, assists10, 
            deaths1, deaths2, deaths3, deaths4, deaths5, deaths6, deaths7, deaths8, deaths9, deaths10, 
  
            dl_turret_status, dl_inhibitor_status, 
            ur_turret_status, ur_inhibitor_status)
            VALUES {mlSb}
            ON DUPLICATE KEY UPDATE
                gold1 = VALUES(gold1),
                gold2 = VALUES(gold2),
                gold3 = VALUES(gold3),
                gold4 = VALUES(gold4),
                gold5 = VALUES(gold5),
                gold6 = VALUES(gold6),
                gold7 = VALUES(gold7),
                gold8 = VALUES(gold8),
                gold9 = VALUES(gold9),
                gold10 = VALUES(gold10),

                kills1 = VALUES(kills1),
                kills2 = VALUES(kills2),
                kills3 = VALUES(kills3),
                kills4 = VALUES(kills4),
                kills5 = VALUES(kills5),
                kills6 = VALUES(kills6),
                kills7 = VALUES(kills7),
                kills8 = VALUES(kills8),
                kills9 = VALUES(kills9),
                kills10 = VALUES(kills10),

                deaths1 = VALUES(deaths1),
                deaths2 = VALUES(deaths2),
                deaths3 = VALUES(deaths3),
                deaths4 = VALUES(deaths4),
                deaths5 = VALUES(deaths5),
                deaths6 = VALUES(deaths6),
                deaths7 = VALUES(deaths7),
                deaths8 = VALUES(deaths8),
                deaths9 = VALUES(deaths9),
                deaths10 = VALUES(deaths10),

                assists1 = VALUES(assists1),
                assists2 = VALUES(assists2),
                assists3 = VALUES(assists3),
                assists4 = VALUES(assists4),
                assists5 = VALUES(assists5),
                assists6 = VALUES(assists6),
                assists7 = VALUES(assists7),
                assists8 = VALUES(assists8),
                assists9 = VALUES(assists9),
                assists10 = VALUES(assists10),

                dl_turret_status = VALUES(dl_turret_status),
                dl_inhibitor_status = VALUES(dl_inhibitor_status),
                ur_turret_status = VALUES(ur_turret_status),
                ur_inhibitor_status = VALUES(ur_inhibitor_status)";
            #endregion

            //var finalSQL = $"START TRANSACTION; {mdqpart}; {pstatsQPart}; {mlPart}; COMMIT;";

            using (var transaction = Adapter.Connection.BeginTransaction())
            {
                try
                {
                    using (var mdCmd = new MySqlCommand(mdQuery, Adapter.Connection, transaction))
                    using (var psCmd = new MySqlCommand(pStatQuery, Adapter.Connection, transaction))
                    using (var mlCmd = new MySqlCommand(mlQuery, Adapter.Connection, transaction))
                    {
                        mdCmd.ExecuteNonQuery();
                        psCmd.ExecuteNonQuery();
                        mlCmd.ExecuteNonQuery();

                        MatchResultDbClass.UpdateMatchStatus(matchResult.Id, Adapter, transaction, 1);

                        transaction.Commit();
                    }
                }
                catch (Exception qException)
                {
                    LoggersAdapter.Info($"Ошибка при проведении транзакции сохранения результатов: {qException}");
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception rollbackException)
                    {
                        LoggersAdapter.Info($"Ошибка при откате транзакции: {rollbackException}");
                    }

                    return false;
                }
            }

            return true;
        }

        private void FillMatchLogData(MatchDetailsResponse response, MatchTimeLineResponse timelineResponse, MatchResultDbClass matchResult)
        {
            var participantStats = response.participants;
            if (participantStats != null)
            {
                foreach (var participantStat in participantStats)
                {
                    var entry = new ParticipantStatsEntry()
                    {
                        ParticipantId = participantStat.participantId,
                        ChampionId = participantStat.championId,
                        TeamId = participantStat.teamId,
                        Spell1 = participantStat.spell1Id,
                        Spell2 = participantStat.spell2Id,
                        Role = participantStat.timeline.role,
                        Lane = participantStat.timeline.lane,
                        Kills = participantStat.stats.kills,
                        Deaths = participantStat.stats.deaths,
                        Assists = participantStat.stats.assists,
                        TotalMinionsKilled = participantStat.stats.totalMinionsKilled,
                        TotalDamageDealt = participantStat.stats.totalDamageDealt,
                        TotalDamageDealtToChampions = participantStat.stats.totalDamageDealtToChampions,
                        MagicDamageDealt = participantStat.stats.magicDamageDealt,
                        MagicDamageDealtToChampions = participantStat.stats.magicDamageDealtToChampions,
                        PhysicalDamageDealt = participantStat.stats.physicalDamageDealt,
                        PhysicalDamageDealtToChampions = participantStat.stats.physicalDamageDealtToChampions,
                        GoldEarned = participantStat.stats.goldEarned,
                        GoldSpent = participantStat.stats.goldSpent,
                        ChampLevel = participantStat.stats.champLevel,
                        WardsPlaced = participantStat.stats.wardsPlaced,
                        WardsKilled = participantStat.stats.wardsKilled,
                    };
                    matchResult.ParticipantStats.Add(entry);
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(participantStats));
            }

            var frames = timelineResponse.frames;
            var interval = timelineResponse.frameInterval;

            var kills = new int[10];
            var deaths = new int[10];
            var assists = new int[10];

            var uprightTurretsStatus = 2047;
            var dwnleftTurretsStatus = 2047;
            var uprightInhibitorsStatus = 7;
            var dwnleftInhibitorsStatus = 7;

            foreach (var frame in frames)
            {
                var gold = new int[10];
                var logEntry = new MatchTimeLineEntry
                {
                    timestamp = TimeSpan.FromMilliseconds(frame.timestamp),
                };

                foreach (var frameParticipantFrame in frame.participantFrames)
                {
                    var participant = frameParticipantFrame.Key; // какой чувак
                    var gld = frameParticipantFrame.Value.totalGold;
                    gold[participant - 1] = gld;
                }

                logEntry.gold = gold.ToArray();
                matchResult.MatchLog.Add(logEntry);

                var frameEvents = frame.events;
                if (frameEvents != null && frameEvents.Length > 0)
                {
                    foreach (var evnt in frameEvents)
                    {
                        ProcessMatchEvent(
                            matchResult, evnt, logEntry,
                            ref kills, ref deaths, ref assists, ref gold,
                            ref dwnleftTurretsStatus, ref dwnleftInhibitorsStatus,
                            ref uprightTurretsStatus, ref uprightInhibitorsStatus);
                    }
                }

                logEntry.DWNLEFT_InhibitorsStatus = dwnleftInhibitorsStatus;
                logEntry.DWNLEFT_TurretsStatus = dwnleftTurretsStatus;
                logEntry.UPRIGHT_InhibitorsStatus = uprightInhibitorsStatus;
                logEntry.UPRIGHT_TurretsStatus = uprightTurretsStatus;
                logEntry.kills = kills.ToArray();
                logEntry.deaths = deaths.ToArray();
                logEntry.assists = assists.ToArray();
            }
        }

        private static void ProcessMatchEvent(MatchResultDbClass matchResult, Event evnt, MatchTimeLineEntry logEntry,
            ref int[] kills, ref int[] deaths, ref int[] assists, ref int[] gold,
            ref int DL_towerStatus, ref int DL_inhibitorsStatus,
            ref int UR_towerStatus, ref int UR_inhibitorsStatus)
        {
            switch (evnt.type)
            {
                case "CHAMPION_KILL":
                    var champKillEntry = logEntry.Clone() as MatchTimeLineEntry;
                    champKillEntry.timestamp = TimeSpan.FromMilliseconds(evnt.timestamp);

                    var killer = evnt.killerId;
                    var victim = evnt.victimId;
                    var assisters = evnt.assistingParticipantIds;

                    if (killer > 0) kills[killer - 1]++;
                    if (victim > 0) deaths[victim - 1]++;
                    if (assisters != null && assisters.Length > 0)
                        foreach (var assister in assisters)
                            if (assister > 0) assists[assister - 1]++;

                    champKillEntry.kills = kills.ToArray();
                    champKillEntry.deaths = deaths.ToArray();
                    champKillEntry.assists = assists.ToArray();
                    champKillEntry.gold = gold.ToArray();

                    champKillEntry.DWNLEFT_InhibitorsStatus = DL_inhibitorsStatus;
                    champKillEntry.DWNLEFT_TurretsStatus = DL_towerStatus;
                    champKillEntry.UPRIGHT_InhibitorsStatus = UR_inhibitorsStatus;
                    champKillEntry.UPRIGHT_TurretsStatus = UR_towerStatus;

                    matchResult.MatchLog.Add(champKillEntry);
                    break;

                case "BUILDING_KILL":
                    var buildKillEntry = logEntry.Clone() as MatchTimeLineEntry;
                    buildKillEntry.timestamp = TimeSpan.FromMilliseconds(evnt.timestamp);

                    if (evnt.buildingType == "INHIBITOR_BUILDING")
                    {
                        if (evnt.teamId == 100) // Убили у радиантов
                        {
                            DL_inhibitorsStatus = BuildingManager.KillInhibitor(evnt.laneType, DL_inhibitorsStatus);
                        }
                        else if (evnt.teamId == 200) // у дайров
                        {
                            UR_inhibitorsStatus = BuildingManager.KillInhibitor(evnt.laneType, UR_inhibitorsStatus);
                        }
                    }
                    else if (evnt.buildingType == "TOWER_BUILDING")
                    {
                        if (evnt.teamId == 100) // Убили у радиантов
                        {
                            var wasNexusDestroyedBefore = (DL_towerStatus >> 10 & 1) == 0;
                            DL_towerStatus = BuildingManager.KillTower(evnt.towerType, evnt.laneType, DL_towerStatus, wasNexusDestroyedBefore);
                        }
                        else if (evnt.teamId == 200) // у дайров
                        {
                            var wasNexusDestroyedBefore = (UR_towerStatus >> 10 & 1) == 0;
                            UR_towerStatus = BuildingManager.KillTower(evnt.towerType, evnt.laneType, UR_towerStatus, wasNexusDestroyedBefore);
                        }
                    }

                    buildKillEntry.DWNLEFT_InhibitorsStatus = DL_inhibitorsStatus;
                    buildKillEntry.DWNLEFT_TurretsStatus = DL_towerStatus;
                    buildKillEntry.UPRIGHT_InhibitorsStatus = UR_inhibitorsStatus;
                    buildKillEntry.UPRIGHT_TurretsStatus = UR_towerStatus;

                    buildKillEntry.kills = kills.ToArray();
                    buildKillEntry.deaths = deaths.ToArray();
                    buildKillEntry.assists = assists.ToArray();
                    buildKillEntry.gold = gold.ToArray();

                    matchResult.MatchLog.Add(buildKillEntry);
                    break;
            }
        }

        private void FillCommonResultData(Team teamDlStats, Team teamUrStats, MatchResultDbClass matchResult)
        {
            // FB
            if (teamDlStats.firstBlood && !teamUrStats.firstBlood)
                matchResult.FirstBlood = teamDlStats.teamId;
            else if (!teamDlStats.firstBlood && teamUrStats.firstBlood)
                matchResult.FirstBlood = teamUrStats.teamId;

            // First dragon
            if (teamDlStats.firstDragon && !teamUrStats.firstDragon)
                matchResult.FirstDragon = teamDlStats.teamId;
            else if (!teamDlStats.firstDragon && teamUrStats.firstDragon)
                matchResult.FirstDragon = teamUrStats.teamId;

            // First tower
            if (teamDlStats.firstTower && !teamUrStats.firstTower)
                matchResult.FirstTower = teamDlStats.teamId;
            else if (!teamDlStats.firstTower && teamUrStats.firstTower)
                matchResult.FirstTower = teamUrStats.teamId;

            // First inhibitor
            if (teamDlStats.firstInhibitor && !teamUrStats.firstInhibitor)
                matchResult.FirstInhibitor = teamDlStats.teamId;
            else if (!teamDlStats.firstInhibitor && teamUrStats.firstInhibitor)
                matchResult.FirstInhibitor = teamUrStats.teamId;

            // First rift
            if (teamDlStats.firstRiftHerald && !teamUrStats.firstRiftHerald)
                matchResult.FirstRiftHeraId = teamDlStats.teamId;
            else if (!teamDlStats.firstRiftHerald && teamUrStats.firstRiftHerald)
                matchResult.FirstRiftHeraId = teamUrStats.teamId;

            // Winner
            if (teamDlStats.win == "Win" && teamUrStats.win == "Fail")
                matchResult.Winner = teamDlStats.teamId;
            else if (teamDlStats.win == "Fail" && teamUrStats.win == "Win")
                matchResult.Winner = teamUrStats.teamId;

            // First baron
            if (teamDlStats.firstBaron && !teamUrStats.firstBaron)
                matchResult.FirstBaron = teamDlStats.teamId;
            else if (!teamDlStats.firstBaron && teamUrStats.firstBaron)
                matchResult.FirstBaron = teamUrStats.teamId;
        }

    }

    public class AramMatchResultsProcessor : IResultSaver
    {
    }
}
