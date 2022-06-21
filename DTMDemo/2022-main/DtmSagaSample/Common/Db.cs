using MySqlConnector;
using System.Data.Common;
using Dapper;

namespace Common
{
    public class Db
    {
        private static readonly string _conn = "Server=127.0.0.1;port=3306;User ID=root;Password=lzh123456;Database=cloudworking";

        public static DbConnection GeConn() => new MySqlConnection(_conn);

        public static async Task<bool> AdjustBalance(string userId, int amount)
        {
            try
            {
                var sql = "update t_user set phone='123'";
                using var conn = GeConn();
                await conn.OpenAsync();
                var affectedRows = await conn.ExecuteAsync(sql, new { userId, amount });
                return affectedRows > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<bool> BarrierAdjustBalance(string userId, int amount, DbTransaction tran)
        {
            try
            {
                var sql = "update t_user set phone='000'";
                using var conn = tran.Connection;
                var affectedRows = await conn.ExecuteAsync(sql, new { userId, amount }, tran);
                return affectedRows > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}