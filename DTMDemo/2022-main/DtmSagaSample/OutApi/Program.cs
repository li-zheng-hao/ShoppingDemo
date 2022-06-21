using Common;
using Dtmcli;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDtmcli(x =>
{
    x.DtmUrl = "http://localhost:36789";
    x.BarrierTableName = "cloudworking.barrier";
});

var app = builder.Build();

app.MapPost("/api/TransOut", (HttpContext httpContext, TransRequest req) => 
{
    Console.WriteLine($"�û�2����");

    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.MapPost("/api/TransOutCompensate", (HttpContext httpContext, TransRequest req) =>
{
    Console.WriteLine($"�û�2�ع�");
    
    return Results.Ok(TransResponse.BuildSucceedResponse());
});

app.Run("http://*:10000");
