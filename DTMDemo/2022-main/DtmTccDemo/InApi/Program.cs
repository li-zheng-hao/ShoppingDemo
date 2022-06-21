using Common;
using Dtmcli;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDtmcli(x => 
{
    x.DtmUrl = "http://localhost:36789";
});

var app = builder.Build();

app.MapPost("/api/TransInTry", async (IBranchBarrierFactory bbFactory, HttpContext context, TransRequest req) =>
{
    var bb = bbFactory.CreateBranchBarrier(context.Request.Query);

    using var db = Db.GeConn();
    await bb.Call(db, async (tx) =>
    {
        Console.WriteLine($"�û���{req.UserId}��ת�롾{req.Amount}��Try������bb={bb}");
        await Task.CompletedTask;
    });

    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.MapPost("/api/TransInConfirm", async (IBranchBarrierFactory bbFactory, HttpContext context, TransRequest req) =>
{
    var bb = bbFactory.CreateBranchBarrier(context.Request.Query);

    using var db = Db.GeConn();
    await bb.Call(db, async (tx) =>
    {
        Console.WriteLine($"�û���{req.UserId}��ת�롾{req.Amount}��Confirm������bb={bb}");
        await Task.CompletedTask;
    });

    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.MapPost("/api/TransInTryError", (IBranchBarrierFactory bbFactory, HttpContext context, TransRequest req) =>
{
    var bb = bbFactory.CreateBranchBarrier(context.Request.Query);

    Console.WriteLine($"�û���{req.UserId}��ת�롾{req.Amount}��Try--ʧ�ܣ�bb={bb}");

    return Results.Ok(TransResponse.BuildFailureResponse());
});

app.MapPost("/api/TransInCancel", async (IBranchBarrierFactory bbFactory, HttpContext context, TransRequest req) =>
{
    var bb = bbFactory.CreateBranchBarrier(context.Request.Query);

    using var db = Db.GeConn();
    await bb.Call(db, async (tx) =>
    {
        Console.WriteLine($"�û���{req.UserId}��ת�롾{req.Amount}��Cancel������bb={bb}");
        await Task.CompletedTask;
    });

    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.Run("http://*:10001");
