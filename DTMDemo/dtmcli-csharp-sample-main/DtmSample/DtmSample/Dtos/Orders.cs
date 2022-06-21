using SqlSugar;

namespace DtmSample.Dtos
{
    [SugarTable("orders")]
    public class Orders
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]//数据库是自增才配自增 
        public int id { get; set; }

        public int num { get; set; }
    }
}
