using Yarp.LoadBalancer.Extensions;
using Yarp.LoadBalancer.Extensions.Middlewares;
using Yarp.LoadBalancer.Service.BLL.ClusterManager.Interfaces;
using Yarp.LoadBalancer.Service.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMultipleImplementingInterface(typeof(IGameClusterManager), typeof(IGameClusterManager).Assembly);
builder.Services.AddMonGoDbClient();
builder.Services.AddSingleton<ITpAuthTokenService, TpAuthTokenService>();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<CustomRoutingMiddleware>();

app.MapReverseProxy();
app.MapHealthChecks("health");

app.Run();
