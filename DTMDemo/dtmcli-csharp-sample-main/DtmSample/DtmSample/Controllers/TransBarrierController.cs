using Dtmcli;
using DtmSample.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySqlConnector;
using System.Data.Common;
using System.Threading.Tasks;
using Dapper;
namespace DtmSample.Controllers
{
    [ApiController]
    [Route("/api")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TransBarrierController : ControllerBase
    {
        private readonly ILogger<TransBarrierController> _logger;
        private readonly AppSettings _settings;
        private readonly IBranchBarrierFactory _barrierFactory;

        public TransBarrierController(ILogger<TransBarrierController> logger, IOptions<AppSettings> optionsAccs, IBranchBarrierFactory barrierFactory)
        {
            _logger = logger;
            _settings= optionsAccs.Value;
            _barrierFactory = barrierFactory;
        }

        private MySqlConnection GetBarrierConn() => new(_settings.BarrierConn);

        #region TCC
        [HttpPost("barrierTransOutTry")]
        public async Task<IActionResult> BarrierTransOutTry([FromQuery] string gid, [FromQuery] string trans_type,
            [FromQuery] string branch_id, [FromQuery] string op, [FromBody] TransRequest body)
        {
            _logger.LogInformation("BarrierTransOutTry, QueryString={0}, body={1}", Request.QueryString, body.ToString());

            var branchBarrier = _barrierFactory.CreateBranchBarrier(Request.Query);

            using (MySqlConnection conn = GetBarrierConn())
            {
                await branchBarrier.Call(conn, async (tx) =>
                {
                    _logger?.LogInformation("用户: {0},转出 {1} 元---准备", body.UserId, body.Amount);
                    await Task.CompletedTask;
                });
            }

            return Ok(TransResponse.BuildSucceedResponse());
        }

        [HttpPost("barrierTransOutConfirm")]
        public async Task<IActionResult> BarrierTransOutConfirm([FromQuery] string gid, [FromQuery] string trans_type,
           [FromQuery] string branch_id, [FromQuery] string op, [FromBody] TransRequest body)
        {
            _logger.LogInformation("BarrierTransOutConfirm, QueryString={0}, body={1}", Request.QueryString, body.ToString());

            var branchBarrier = _barrierFactory.CreateBranchBarrier(Request.Query);

            using (MySqlConnection conn = GetBarrierConn())
            {
                await branchBarrier.Call(conn, async (tx) =>
                {
                    _logger?.LogInformation("用户: {0},转出 {1} 元---提交", body.UserId, body.Amount);
                    await Task.CompletedTask;
                });
            }

            return Ok(TransResponse.BuildSucceedResponse());
        }

        [HttpPost("barrierTransOutCancel")]
        public async Task<IActionResult> BarrierTransOutCancel([FromQuery] string gid, [FromQuery] string trans_type,
           [FromQuery] string branch_id, [FromQuery] string op, [FromBody] TransRequest body)
        {
            _logger.LogInformation("BarrierTransOutCancel, QueryString={0}, body={1}", Request.QueryString, body.ToString());

            var branchBarrier = _barrierFactory.CreateBranchBarrier(Request.Query);

            using (MySqlConnection conn = GetBarrierConn())
            {
                await branchBarrier.Call(conn, async (tx) =>
                {
                    _logger?.LogInformation("用户: {0},转出 {1} 元---回滚", body.UserId, body.Amount);
                    await Task.CompletedTask;
                });
            }

            return Ok(TransResponse.BuildSucceedResponse());
        }

        [HttpPost("barrierTransInTry")]
        public async Task<IActionResult> BarrierTransInTry([FromQuery] string gid, [FromQuery] string trans_type,
            [FromQuery] string branch_id, [FromQuery] string op, [FromBody] TransRequest body)
        {
            _logger.LogInformation("BarrierTransInTry, QueryString={0}, body={1}", Request.QueryString, body.ToString());

            var branchBarrier = _barrierFactory.CreateBranchBarrier(Request.Query);

            using (MySqlConnection conn = GetBarrierConn())
            {
                await branchBarrier.Call(conn, async (tx) =>
                {
                    _logger?.LogInformation("用户: {0},转入 {1} 元---准备", body.UserId, body.Amount);
                    await Task.CompletedTask;
                });
            }

            return Ok(TransResponse.BuildSucceedResponse());
        }

        [HttpPost("barrierTransInTryError")]
        public async Task<IActionResult> BarrierTransInTryError([FromQuery] string gid, [FromQuery] string trans_type,
            [FromQuery] string branch_id, [FromQuery] string op, [FromBody] TransRequest body)
        {
            _logger.LogInformation("barrierTransInTryError, QueryString={0}, body={1}", Request.QueryString, body.ToString());

            /*var branchBarrier = _barrierFactory.CreateBranchBarrier(trans_type, gid, branch_id, op);

            using (MySqlConnection conn = GetBarrierConn())
            {
                await branchBarrier.Call(conn, async (tx) =>
                {
                    _logger?.LogInformation("用户: {0},转入 {1} 元---准备", body.UserId, body.Amount);
                    await Task.CompletedTask;
                });
            }*/
            await Task.Yield();
            return BadRequest();
        }

        [HttpPost("barrierTransInConfirm")]
        public async Task<IActionResult> BarrierTransInConfirm([FromQuery] string gid, [FromQuery] string trans_type,
           [FromQuery] string branch_id, [FromQuery] string op, [FromBody] TransRequest body)
        {
            _logger.LogInformation("BarrierTransInConfirm, QueryString={0}, body={1}", Request.QueryString, body.ToString());

            var branchBarrier = _barrierFactory.CreateBranchBarrier(Request.Query);

            using (MySqlConnection conn = GetBarrierConn())
            {
                await branchBarrier.Call(conn, async (tx) =>
                {
                    _logger?.LogInformation("用户: {0},转入 {1} 元---提交", body.UserId, body.Amount);
                    await Task.CompletedTask;
                });
            }

            return Ok(TransResponse.BuildFailureResponse());
        }

        [HttpPost("barrierTransInCancel")]
        public async Task<IActionResult> BarrierTransInCancel([FromQuery] string gid, [FromQuery] string trans_type,
           [FromQuery] string branch_id, [FromQuery] string op, [FromBody] TransRequest body)
        {
            _logger.LogInformation("BarrierTransInCancel, QueryString={0}, body={1}", Request.QueryString, body.ToString());

            var branchBarrier = _barrierFactory.CreateBranchBarrier(Request.Query);

            using (MySqlConnection conn = GetBarrierConn())
            {
                await branchBarrier.Call(conn, async (tx) =>
                {
                    _logger?.LogInformation("用户: {0},转入 {1} 元---回滚", body.UserId, body.Amount);
                    await Task.CompletedTask;
                });
            }

            return Ok(TransResponse.BuildSucceedResponse());
        }
        #endregion

        #region SAGA
        [HttpPost("barrierTransOutSaga")]
        public async Task<IActionResult> BarrierTransOutSaga([FromQuery] string gid, [FromQuery] string trans_type,
            [FromQuery] string branch_id, [FromQuery] string op, [FromBody] TransRequest body)
        {

            var branchBarrier = _barrierFactory.CreateBranchBarrier(Request.Query);

            using (MySqlConnection conn = GetBarrierConn())
            {
                await branchBarrier.Call(conn, async (tx) =>
                {
                    try
                    {
                        var db=SugarClient.GetSqlSugarClient;
                        db.Updateable<Orders>().SetColumns(it => it.num == it.num - 100).Where(it => it.id == 1).ExecuteCommand();
                    }
                    catch (System.Exception ex)
                    {

                        throw;
                    }
                   
                    _logger?.LogInformation("用户: {0},转出 {1} 元---正向操作", body.UserId, body.Amount);
                    await Task.CompletedTask;
                });
            }

            return Ok(TransResponse.BuildSucceedResponse());
        }

        [HttpPost("barrierTransOutSagaRevert")]
        public async Task<IActionResult> BarrierTransOutSagaRevert([FromQuery] string gid, [FromQuery] string trans_type,
          [FromQuery] string branch_id, [FromQuery] string op, [FromBody] TransRequest body)
        {
            
            var branchBarrier = _barrierFactory.CreateBranchBarrier(Request.Query);

            using (MySqlConnection conn = GetBarrierConn())
            {
                await branchBarrier.Call(conn, async (tx) =>
                {
                    var db = SugarClient.GetSqlSugarClient;
                    db.Updateable<Orders>().SetColumns(it => it.num == it.num + 100).Where(it => it.id == 1).ExecuteCommand();
                    _logger?.LogInformation("用户: {0},转出 {1} 元---回滚", body.UserId, body.Amount);
                    await Task.CompletedTask;
                });
            }

            return Ok(TransResponse.BuildSucceedResponse());
        }

        [HttpPost("barrierTransInSaga")]
        public async Task<IActionResult> BarrierTransInSaga([FromQuery] string gid, [FromQuery] string trans_type,
            [FromQuery] string branch_id, [FromQuery] string op, [FromBody] TransRequest body)
        {

            var branchBarrier = _barrierFactory.CreateBranchBarrier(trans_type, gid, branch_id, op);
            bool success = true;
            using (MySqlConnection conn = GetBarrierConn())
            {
                await branchBarrier.Call(conn, async (tx) =>
                {
                    using var db = SugarClient.GetSqlSugarClient;
                    db.BeginTran();
                    try
                    {

                        db.Updateable<Orders>().SetColumns(it => it.num == it.num + 100).Where(it => it.id == 2).ExecuteCommand();
                        throw new System.Exception("异常");
                        db.CommitTran();
                    }
                    catch (System.Exception)
                    {
                        db.RollbackTran();
                       
                        success = false;
                    }
                   
                    _logger?.LogInformation("用户: {0},转入 {1} 元---正向操作", body.UserId, body.Amount);
                    await Task.CompletedTask;
                });
            }

            // 模拟转入失败，触发转入空补偿
            //await Task.Yield();
            //return BadRequest("自定义错误信息");
            //return Ok(TransResponse.BuildFailureResponse());
            return Ok(success?TransResponse.BuildSucceedResponse():TransResponse.BuildFailureResponse());
        }

        [HttpPost("barrierTransInSagaRevert")]
        public async Task<IActionResult> BarrierTransInSagaRevert([FromQuery] string gid, [FromQuery] string trans_type,
            [FromQuery] string branch_id, [FromQuery] string op, [FromBody] TransRequest body)
        {

            var branchBarrier = _barrierFactory.CreateBranchBarrier(Request.Query);

            using (MySqlConnection conn = GetBarrierConn())
            {
                await branchBarrier.Call(conn, async (tx) =>
                {
                    // 用户 1 转出 30 成功
                    // 用户 2 转入 30 失败
                    // 用户 2 转入 30, 不应该补偿，也就是不会打印下面的日志
                    // 用户 1 转出 30, 应该补偿。
                    var db = SugarClient.GetSqlSugarClient;
                    db.Updateable<Orders>().SetColumns(it => it.num == it.num - 100).Where(it => it.id == 2).ExecuteCommand();
                    _logger?.LogInformation("用户: {0},转入 {1} 元---回滚", body.UserId, body.Amount);
                    await Task.CompletedTask;
                });
            }

            return Ok(TransResponse.BuildSucceedResponse());
        } 
        #endregion
    }
}
