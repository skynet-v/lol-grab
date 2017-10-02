using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace LoLApiParser
{
    public class Utils
    {
        public static DateTime FromMilisecEpochTime(long epochTime)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dtDateTime.AddMilliseconds(epochTime);
        }

        public static DateTime FromSecEpochTime(long epochTime)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dtDateTime.AddSeconds(epochTime);
        }

        private void UpdateMatchStatus(long matchId, DbAdapterBase Adapter, MySqlTransaction transaction, int status = 3)
        {
            var q = $"UPDATE MatchDescription md SET md.parse_status = {status} WHERE md.id = {matchId};";
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
}
