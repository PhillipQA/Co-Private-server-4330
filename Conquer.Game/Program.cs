global using Conquer.Game.Data;
global using Conquer.Game.Messages;
global using Conquer.Game.Models;
global using Conquer.Game.Scripting;
global using Conquer.Game.Services;
global using Conquer.Protocols;
global using static System.Buffers.Binary.BinaryPrimitives;
using System.Security.Cryptography;
using Conquer;
using Conquer.Game;
using Conquer.Game.Workers;
using Conquer.Middleware;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddPooledDbContextFactory<GameDbContext>(options => options.UseSqlServer(connectionString)
    .ConfigureWarnings(warnings => warnings.Log(
        (RelationalEventId.ConnectionError, LogLevel.Debug),
        (RelationalEventId.CommandError, LogLevel.Debug),
        (CoreEventId.SaveChangesFailed, LogLevel.Debug),
        (CoreEventId.QueryIterationFailed, LogLevel.Debug))));

builder.Services.AddMemoryCache(options => options.SizeLimit = 10_000);

builder.Services
    .AddConquer<GameClient>()
    .AddSingleton<World>()
    .AddSingleton<TimeProvider, GameClock>()
    .AddSingleton<CommandService>()
    .AddSingleton<GameData>()
    .AddSingleton<GameStore>()
    .AddSingleton<ScriptService>()
    .AddSingleton<GuildService>();

builder.Services.AddHostedService<WorldService>();

builder.Services.AddOptions<GameServerOptions>().BindConfiguration("GameServer");

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5816, listenOptions =>
    {
        listenOptions.UseConquer();

        if (builder.Environment.IsDevelopment())
        {
            listenOptions.UseConnectionLogging();
        }

        listenOptions.UseConnectionHandler<GameServer>();
    });

    options.ListenLocalhost(5817);
});

var app = builder.Build();

app.MapGet("/transfer/{userId}", (string userId, IMemoryCache cache) =>
{
    Span<byte> buffer = stackalloc byte[sizeof(ulong)];
    RandomNumberGenerator.Fill(buffer);
    var token = ReadUInt64LittleEndian(buffer);
    cache.Set($"Transfer:{token}", userId, new MemoryCacheEntryOptions { Size = 1, AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1) });
    return token;
});

app.Run();