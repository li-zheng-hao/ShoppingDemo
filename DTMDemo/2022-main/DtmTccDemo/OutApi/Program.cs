using Common;
using Dtmcli;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDtmcli(x =>
{
    x.DtmUrl = "http://localhost:36789";
});

var app = builder.Build();

app.MapPost("/api/TransOutTry", async (IBranchBarrierFactory bbFactory, HttpContext context, TransRequest req) => 
{
    var bb = bbFactory.CreateBranchBarrier(context.Request.Query);

    using var db = Db.GeConn();
    await bb.Call(db, async (tx) =>
    {
        Console.WriteLine($"�û���{req.UserId}��ת����{req.Amount}��Try ������bb={bb}");
        // tx ���������񣬿ɺͱ�������һ���ύ�ع�
        await Task.CompletedTask;
    });

    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.MapPost("/api/TransOutConfirm", async (IBranchBarrierFactory bbFactory, HttpContext context, TransRequest req) =>
{
    var bb = bbFactory.CreateBranchBarrier(context.Request.Query);

    using var db = Db.GeConn();
    await bb.Call(db, async (tx) =>
    {
        Console.WriteLine($"�û���{req.UserId}��ת����{req.Amount}��Confirm������bb={bb}");
        await Task.CompletedTask;
    });

    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.MapPost("/api/TransOutCancel", async (IBranchBarrierFactory bbFactory, HttpContext context, TransRequest req) =>
{
    var bb = bbFactory.CreateBranchBarrier(context.Request.Query);

    using var db = Db.GeConn();
    await bb.Call(db, async (tx) =>
    {
        Console.WriteLine($"�û���{req.UserId}��ת����{req.Amount}��Cancel������bb={bb}");
        await Task.CompletedTask;
    });

    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.Run("http://*:10000");
