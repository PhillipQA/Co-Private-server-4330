global using Conquer.Account.Data;
global using Conquer.Account.Models;
global using Conquer.Protocols;
global using static System.Buffers.Binary.BinaryPrimitives;
using Conquer;
using Conquer.Account;
using Conquer.Cryptography;
using Conquer.Middleware;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextPool<AccountDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentityCore<User>(options => options.Password.RequireNonAlphanumeric = false)
    .AddEntityFrameworkStores<AccountDbContext>();

builder.Services.AddHttpClient();

builder.Services
    .AddConquer<AccountClient>()
    .AddTransient<IPasswordCipher, Rc5PasswordCipher>();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(9958, listenOptions =>
    {
        listenOptions.UseConquer();

        if (builder.Environment.IsDevelopment())
        {
            listenOptions.UseConnectionLogging();
        }

        listenOptions.UseConnectionHandler<AccountServer>();
    });
});

var app = builder.Build();

app.Run();