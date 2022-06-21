using SqlSugar;

namespace DtmSample
{
    public static class SugarClient
    {
        //创建数据库对象
        public static SqlSugarClient GetSqlSugarClient => new SqlSugarClient(new ConnectionConfig()
        {
            ConnectionString = "Server=localhost;port=49154;User ID=root;Password=mysqlpw;Database=dtm_barrier",
            DbType = DbType.MySql,
            IsAutoCloseConnection = true
        });
    }
}
