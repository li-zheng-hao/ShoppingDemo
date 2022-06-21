using Common;
using Dtmcli;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDtmcli(x => 
{
    x.DtmUrl = "http://localhost:36789";
    x.BarrierTableName = "cloudworking.barrier";
});

var app = builder.Build();

app.MapPost("/api/TransIn", (HttpContext httpContext, TransRequest req) =>
{
    Console.WriteLine("用户1正向");
    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.MapPost("/api/TransInCompensate", (HttpContext httpContext, TransRequest req) =>
{

    Console.WriteLine($"用户1回滚");
    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.MapPost("/api/TransInError", (HttpContext httpContext, TransRequest req) =>
{
    Console.WriteLine($"用户1正向出错了！");

    // status code = 409 || content contains FAILURE
    // return Ok(TransResponse.BuildFailureResponse());
    return Results.StatusCode(409);
});

int _errCount = 0;

app.MapPost("/api/BarrierTransIn", async (HttpContext httpContext, TransRequest req, IBranchBarrierFactory factory) =>
{
    var barrier = factory.CreateBranchBarrier(httpContext.Request.Query);

    using var db = Db.GeConn();
    await barrier.Call(db, async (tx) =>
    {

       

        Console.WriteLine($"用户1正向");
        
        await Task.CompletedTask;
    });

    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.MapPost("/api/BarrierTransInCompensate", async (HttpContext httpContext, TransRequest req, IBranchBarrierFactory factory) =>
{
    // create barrier from query
    var barrier = factory.CreateBranchBarrier(httpContext.Request.Query);

    using var db = Db.GeConn();
    await barrier.Call(db, async (tx) =>
    {
        // some exception occure, should not output this one.
        Console.WriteLine($"用户1回滚");

        await Task.CompletedTask;
    });

    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.Run("http://*:10001");
