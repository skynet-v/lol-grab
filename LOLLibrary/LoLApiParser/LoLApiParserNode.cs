using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LOLApiAdapter.ApiHandlers;
using LOLApiAdapter.ApiResponseStructures;
using LOLApiAdapter.CommonDefinitions.Enums;
using MySql.Data.MySqlClient;

namespace LoLApiParser
{
    public class LoLApiParserNode
    {
        private const string ApiKey = "API_KEY";
        private readonly LoLDbAdapter Adapter = new LoLDbAdapter("", "");

        private readonly LolApiServerRegion _parsingRegion;
        private string ApiRegionString => _parsingRegion.ToString().ToLower();


        private readonly SpectatorDataHandler _spectatorHandler;
        private readonly MatchDetailsDataHandler _matchDetailsHandler;

        private CancellationTokenSource _baseRoutineCancellationTokenSource;
        private Task _baseRoutine;
        private CancellationTokenSource _matchResultsRoutineCancellationTokenSource;
        private Task _matchResultsRoutine;
        private CancellationTokenSource _matchTimelineRoutineTokenSource;
        private Task _matchTimelineRoutine;

        public bool MatchParserIsWorking => 
            _baseRoutine?.Status == TaskStatus.Running ||
            _baseRoutine?.Status == TaskStatus.WaitingForActivation ||
            _baseRoutine?.Status == TaskStatus.WaitingToRun;

        public bool MatchResultsParserIsWorking => 
            _matchResultsRoutine?.Status == TaskStatus.Running ||
            _matchResultsRoutine?.Status == TaskStatus.WaitingForActivation ||
            _matchResultsRoutine?.Status == TaskStatus.WaitingToRun;

        public LoLApiParserNode(LolApiServerRegion region)
        {
            _parsingRegion = region;
            _spectatorHandler = new SpectatorDataHandler(ApiKey, ApiRegionString);
            _matchDetailsHandler = new MatchDetailsDataHandler(ApiKey, ApiRegionString);
        }

        public void Start()
        {
            if (_baseRoutine == null || _baseRoutine?.Status == TaskStatus.Canceled || _baseRoutine?.Status == TaskStatus.Faulted || !MatchParserIsWorking ||
                _matchResultsRoutine == null || _matchResultsRoutine?.Status == TaskStatus.Canceled || _matchResultsRoutine?.Status == TaskStatus.Faulted || !MatchResultsParserIsWorking)
            {
                _baseRoutineCancellationTokenSource = new CancellationTokenSource();
                _baseRoutine = Task.Run(() =>
                {
                    while (!_baseRoutineCancellationTokenSource.IsCancellationRequested)
                    {
                        var timeout = TimeSpan.FromSeconds(10);
                        try
                        {
                            LoggersAdapter.Info($"Старт работы LOL парсера для региона {_parsingRegion}.....");

                            Thread.Sleep(2000);
                            var response = FeaturedGamesParseRoutine();
                            if (response != null)
                                timeout = TimeSpan.FromSeconds(response.clientRefreshInterval);
                        }
                        catch (Exception e)
                        {
                            LoggersAdapter.Info($"Исключение в базовой рабочей задаче (повтор через {timeout.TotalSeconds:F1} секунд): {e}");
                        }

                        Thread.Sleep(timeout);
                    }
                }, _baseRoutineCancellationTokenSource.Token);

                _matchResultsRoutineCancellationTokenSource = new CancellationTokenSource();
                _matchResultsRoutine = Task.Run(() =>
                {
                    Thread.Sleep(10000);
                    while (!_matchResultsRoutineCancellationTokenSource.IsCancellationRequested)
                    {
                        var timeout = TimeSpan.FromSeconds(30);
                        LoggersAdapter.Info($"Тик MatchResultsRoutine для региона {_parsingRegion}.....");
                        
                        var matchesToParse = GetMatchesToParse(ApiRegionString);
                        foreach (var match in matchesToParse)
                        {
                            try
                            {
                                Thread.Sleep(2000);
                                MatchResultsParseRoutine(match);
                                LoggersAdapter.Debug($"Обработан матч {match}");
                                Thread.Sleep(2000);
                            }
                            catch (Exception e)
                            {
                                LoggersAdapter.Info(
                                    $"Исключение в >MatchResults< рабочей задаче (повтор через {timeout.TotalSeconds:F1} секунд): {e}");
                            }
                        }

                        Thread.Sleep(timeout);
                    }
                }, _matchResultsRoutineCancellationTokenSource.Token);
            }
        }

        public void Stop()
        {
            if(MatchParserIsWorking)
                _baseRoutineCancellationTokenSource.Cancel();
            if(MatchResultsParserIsWorking)
                _matchResultsRoutineCancellationTokenSource.Cancel();
        }

        private IEnumerable<long> GetMatchesToParse(string region)
        {
            var result = new List<long>();
            try
            {
                var stm = $"CALL lol.LoLApiParser_GetMatchesToParse(\"{region}\");";
                using (var cmd = new MySqlCommand(stm, Adapter.Connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(reader.GetInt64("id"));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LoggersAdapter.Info($"Ошибка при получении списка матчей: {e}");
            }

            return result.ToArray();
        }

        private void MatchResultsParseRoutine(long matchId)
        {
            var response = GetMatchByMatchId(_matchDetailsHandler, matchId);
            if (response != null)
            {
                LoggersAdapter.Debug($"Получен ответ по резам матча для региона {_parsingRegion}");
                //ProcessMatchResultsResponse(response);
            }
            else
                LoggersAdapter.Debug($"Получен пустой ответ по матчу {matchId} от api-сервера для региона {_parsingRegion}");

            var timeLineResponse = GetMatchTimeLineByMatchId(_matchDetailsHandler, matchId);
            if (timeLineResponse != null)
            {
                LoggersAdapter.Debug($"Получен ответ по таймлайну матча для региона {_parsingRegion}");
                //ProcessMatchTimelineResponse(timeLineResponse);
            }
            else
                LoggersAdapter.Debug($"Получен пустой ответ по по таймлайну матча {matchId} от api-сервера для региона {_parsingRegion}");

            if (response != null && timeLineResponse != null)
            {
                switch (response.gameMode)
                {
                    case "CLASSIC":
                        new ClassicMatchResultsProcessor().ProcessClassicMatchResultsResponse(response, timeLineResponse, Adapter);
                        break;
                    case "ARAM":
                        throw new NotImplementedException();
                        //new AramMatchResultsProcessor();
                        break;
                }
                
            }
            else
            {
                LoggersAdapter.Info(
                    $"Одна из полученных структур результатов матча пуста - сохранения матча {matchId} не произойдет");
                MatchResultDbClass.UpdateMatchStatus(matchId, Adapter, null, 3);
            }
        }

        private SpectatorGamesResponse FeaturedGamesParseRoutine()
        {
            var response = GetFeaturedMatches(_spectatorHandler);
            if (response != null)
            {
                LoggersAdapter.Debug($"Получен ответ для региона {_parsingRegion}");
                ProcessFeaturedGamesResponse(response);
            }
            else
                LoggersAdapter.Debug($"Получен пустой ответ от api-сервера для региона {_parsingRegion}");

            return response;
        }

        private void ProcessFeaturedGamesResponse(SpectatorGamesResponse response)
        {
            if (response == null)
            {
                LoggersAdapter.Warn($"Пришел пустой {nameof(response)}");
                throw new ArgumentNullException(nameof(response));
            }

            using (var transaction = Adapter.Connection.BeginTransaction())
            {
                try
                {
                    foreach (var gameMatch in response.gameList) // .Where(x => x.participants.All(part => !part.bot)) TODO: нужно или нет
                    {
                        const string q = "INSERT INTO lol.MatchDescription (id, dt_insert, dt_start, platform_id, game_mode, map_id) " +
                                         "VALUES (@game_id, UTC_TIMESTAMP(), @game_start_time, @platform_id, @game_mode, @map_id) " +
                                         "ON DUPLICATE KEY UPDATE id = id;";

                        var cmd = new MySqlCommand(q)
                        {
                            Connection = Adapter.Connection,
                            Transaction = transaction,
                        };
                        cmd.Parameters.AddWithValue("@game_id", gameMatch.gameId);
                        cmd.Parameters.AddWithValue("@game_start_time", Utils.FromMilisecEpochTime(gameMatch.gameStartTime));
                        cmd.Parameters.AddWithValue("@platform_id", gameMatch.platformId);
                        cmd.Parameters.AddWithValue("@game_mode", gameMatch.gameMode);
                        cmd.Parameters.AddWithValue("@map_id", gameMatch.mapId);

                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (MySqlException ex)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e)
                    {
                        LoggersAdapter.Info($"Ошибка при попытке отката изменений в БД: {e}");
                    }
                }
                catch (Exception e)
                {
                    LoggersAdapter.Info($"Ошибка при попытке добавить новые матчи: {e}");
                }
            }
        }

        // Оберточки
        private SpectatorGamesResponse GetFeaturedMatches(SpectatorDataHandler apiDataHandler)
        {
            var response = apiDataHandler.GetFeaturedGames() as SpectatorGamesResponse;
            return response;
        }

        private MatchDetailsResponse GetMatchByMatchId(MatchDetailsDataHandler apiDataHandler, long matchId)
        {
            var response = apiDataHandler.GetMatchByMatchId(matchId) as MatchDetailsResponse;
            return response;
        }

        private MatchTimeLineResponse GetMatchTimeLineByMatchId(MatchDetailsDataHandler matchDetailsHandler, long matchId)
        {
            var response = matchDetailsHandler.GetMatchTimelineByMatchId(matchId) as MatchTimeLineResponse;
            return response;
        }
    }
}
