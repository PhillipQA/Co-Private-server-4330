# Conquer

A Conquer Online private server for patch 4330. Built with C#, ASP.NET Core, and Entity Framework Core.

## Requirements

- .NET 11 SDK
- SQL Server
- A Conquer Online 2.0 patch 4330 client

## Setup

1. Point `ConnectionStrings:DefaultConnection` in `Conquer.Account/appsettings.json` and
   `Conquer.Game/appsettings.json` at your database.
2. Set `GameServer:ClientPath` in `Conquer.Game/appsettings.json` to your client folder. The game server loads its
   maps from there.
3. Create the database and load the world:

   ```
   dotnet tool restore
   dotnet ef database update --project Conquer.Account
   dotnet ef database update --project Conquer.Game
   sqlcmd -S "(localdb)\MSSQLLocalDB" -d Conquer -i seed.sql
   ```

4. Start both servers:

   ```
   dotnet run --project Conquer.Account
   dotnet run --project Conquer.Game
   ```

5. Set the server's IP in the client's `Server.dat`, for example `Ip1=127.0.0.1`. The account server listens on port
   9958 and the game server on 5816. To host for other machines, also change `IpAddress` in the `Servers` table, which
   the account server hands to the client.

## Accounts

| Account | Password  | Character   |
|---------|-----------|-------------|
| Test1   | Password1 | `Test1[GM]` |
| Test2   | Password2 | `Test2[PM]` |
| Test3   | Password3 | none        |

Characters with `[GM]` or `[PM]` in their name can use the `/` commands.
