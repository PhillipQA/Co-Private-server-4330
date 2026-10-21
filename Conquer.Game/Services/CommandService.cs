using System.Collections.Frozen;
using System.Globalization;

namespace Conquer.Game.Services;

public class CommandService(GameData data, World world, GameStore store, IHostApplicationLifetime lifetime)
{
    private const int DropSpread = 12;
    private const int RestartCountdown = 15;
    private const int RestartStep = 5;
    private const uint MaxRestartCountdown = 3600;

    private static readonly FrozenDictionary<string, (uint Map, ushort X, ushort Y)> Cities =
        new Dictionary<string, (uint, ushort, ushort)>
        {
            ["tc"] = (1002, 431, 379),
            ["am"] = (1020, 567, 576),
            ["dc"] = (1000, 500, 650),
            ["mc"] = (1001, 316, 642),
            ["bi"] = (1015, 723, 573),
            ["pc"] = (1011, 190, 271),
            ["ma"] = (1036, 292, 236),
            ["arena"] = (1005, 52, 69)
        }.ToFrozenDictionary();

    private static readonly FrozenDictionary<string, AttributeType> Attributes =
        new Dictionary<string, AttributeType>
        {
            ["str"] = AttributeType.Strength,
            ["agi"] = AttributeType.Agility,
            ["vit"] = AttributeType.Vitality,
            ["spi"] = AttributeType.Spirit
        }.ToFrozenDictionary();

    private static readonly FrozenDictionary<string, string> Usages = new Dictionary<string, string>
    {
        ["/attr"] = "/attr {str|agi|vit|spi} {points}",
        ["/avatar"] = "/avatar {id}",
        ["/ban"] = "/ban {player}",
        ["/dialog"] = "/dialog {id}",
        ["/drop"] = "/drop {item} [count]",
        ["/goto"] = "/goto {player}",
        ["/hair"] = "/hair {style}",
        ["/hp"] = "/hp {amount}",
        ["/item"] = "/item {name|id} [quality] [plus] [ident] [gem1] [gem2] [bless] [enchant] [magic1] [magic2]",
        ["/jail"] = "/jail {player}",
        ["/job"] = "/job {profession}",
        ["/kill"] = "/kill [player]",
        ["/level"] = "/level {level}",
        ["/magic"] = "/magic {type} {level}",
        ["/map"] = "/map {map} {x} {y}",
        ["/mapargb"] = "/mapargb {aarrggbb}",
        ["/model"] = "/model {model}",
        ["/money"] = "/money {amount}",
        ["/mp"] = "/mp {amount}",
        ["/pet"] = "/pet {monster}",
        ["/rebirth"] = "/rebirth {count}",
        ["/recall"] = "/recall {player}",
        ["/scroll"] = $"/scroll {{{string.Join('|', Cities.Keys)}}}",
        ["/skill"] = "/skill {type} {level}",
        ["/spawn"] = "/spawn {monster} [count]",
        ["/status"] = "/status {flag}",
        ["/weather"] = "/weather {type}",
        ["/xp"] = "/xp {0-100}"
    }.ToFrozenDictionary();

    public async Task HandleAsync(Player player, string command)
    {
        var args = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var now = world.UtcNow;

        switch (args[0])
        {
            case "/attr" when Word(1) is { } which && Attributes.TryGetValue(which, out var attribute) && Number(2) is { } points:
                _ = attribute switch
                {
                    AttributeType.Strength => player.Strength = (ushort)points,
                    AttributeType.Agility => player.Agility = (ushort)points,
                    AttributeType.Vitality => player.Vitality = (ushort)points,
                    _ => player.Spirit = (ushort)points
                };

                player.RecalculateStats();
                player.Send(new MsgUserAttrib(player.Id, attribute, points));
                break;

            case "/avatar" when Number(1) is { } avatar:
                player.Avatar = (ushort)avatar;
                player.Show(AttributeType.Look, player.Look);
                break;

            case "/ban" when Word(1) is { } name:
                if (!await store.BanAsync(name))
                {
                    player.Tell($"There is no {name}.");
                    break;
                }

                world.Find(name)?.Client.Close();
                world.Announce($"{name} has been banned!", TalkChannel.Gm);
                break;

            case "/clearinv":
                foreach (var item in player.Inventory.ToList())
                {
                    player.Take(item);
                }
                break;

            case "/cyclone":
                Toggle(PlayerStatus.Cyclone);
                break;

            case "/dialog" when Number(1) is { } dialog:
                player.OpenDialog(dialog);
                break;

            case "/drop" when Find(Word(1)) is { } type:
                var dropped = Scatter((int)(Number(2) ?? 1),
                    (x, y) => player.Map!.Drop(new MapItem { Item = Item.Of(type), OwnerId = player.Id }, x, y, now));

                player.Tell($"Scattered {dropped} {type.Name}.");
                break;

            case "/gm":
                world.Players.Values.Send(new MsgTalk(TalkChannel.Gm, player.Name, "ALLUSERS", "", command[args[0].Length..].Trim()));
                break;

            case "/goto" when world.Find(Word(1) ?? "") is { Map: not null } target:
                player.Teleport(target.MapId, target.X, target.Y, force: true);
                break;

            case "/hair" when Number(1) is { } hair:
                player.SetHair((ushort)hair);
                break;

            case "/hp" when Number(1) is { } health:
                player.AddHealth((int)health - player.Health);
                break;

            case "/item" when Find(Word(1), Number(2)) is { } type:
                player.Give(type, item =>
                {
                    item.Plus = (byte)(Number(3) ?? 0);
                    item.Ident = (byte)(Number(4) ?? 0);
                    item.Gem1 = (Gem)(Number(5) ?? 0);
                    item.Gem2 = (Gem)(Number(6) ?? 0);
                    item.Bless = (byte)(Number(7) ?? 0);
                    item.Enchant = (byte)(Number(8) ?? 0);
                    item.Magic1 = (byte)(Number(9) ?? item.Magic1);
                    item.Magic2 = (byte)(Number(10) ?? item.Magic2);
                });
                break;

            case "/jail" when Word(1) is { } name:
                if (world.Find(name) is { } inmate)
                {
                    inmate.Jail();
                }
                else if (!await store.JailAsync(name, Player.JailCell.Map, Player.JailCell.X, Player.JailCell.Y))
                {
                    player.Tell($"There is no {name}.");
                }
                break;

            case "/job" when Number(1) is { } profession:
                player.SetProfession((Profession)profession);
                break;

            case "/kill" when (Word(1) is { } name ? world.Find(name) : player) is { Map: not null } target:
                target.DieInstantly(player, now);
                break;

            case "/level" when Number(1) is { } level:
                player.SetLevel((byte)level);
                break;

            case "/magic" when Number(1) is { } type && Number(2) is { } level:
                player.SetMagic((ushort)type, (byte)level);
                break;

            case "/map" when Number(1) is { } mapId && Number(2) is { } x && Number(3) is { } y:
                player.Teleport(mapId, (ushort)x, (ushort)y, force: true);
                break;

            case "/mapargb" when Word(1) is { } hex && uint.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var argb):
                player.Send(new MsgAction { Id = player.Id, Data = argb, Action = ActionType.MapColor });
                break;

            case "/model" when Number(1) is { } model:
                player.Model = model;
                player.Show(AttributeType.Look, player.Look);
                break;

            case "/money" when Number(1) is { } money:
                player.Money = 0;
                player.Earn(Math.Min(money, Player.MoneyLimit));
                break;

            case "/mp" when Number(1) is { } mana:
                player.AddMana((int)mana - player.Mana);
                break;

            case "/online":
                player.Tell($"{world.Players.Count} players online.");
                break;

            case "/pet" when Word(1) is null:
                player.DismissPet();
                break;

            case "/pet" when Monster(Word(1)!) is { } type:
                player.SummonPet(type, now.AddHours(1), player.X, player.Y);
                break;

            case "/quit":
                player.Client.Close();
                break;

            case "/reallot":
                player.RedistributePoints();
                player.RecalculateStats();
                player.SendAttributes();
                break;

            case "/rebirth" when Number(1) is { } rebirths:
                player.Rebirths = (byte)rebirths;
                player.RecalculateStats();
                player.Show(AttributeType.Rebirths, rebirths);
                break;

            case "/recall" when world.Find(Word(1) ?? "") is { Map: not null } target && player.Map is not null:
                target.Teleport(player.MapId, player.X, player.Y, force: true);
                break;

            case "/refill":
                player.Refill();
                break;

            case "/restart":
                var seconds = (int)Math.Min(Number(1) ?? RestartCountdown, MaxRestartCountdown);

                for (var left = seconds; left > 0; left -= RestartStep)
                {
                    var warning = $"Server maintenance in {left} seconds! Please log off to avoid losing data.";
                    world.After(TimeSpan.FromSeconds(seconds - left), () => world.Announce(warning, TalkChannel.Gm));
                }

                world.After(TimeSpan.FromSeconds(seconds), () =>
                {
                    world.Announce("The server will shut down now!", TalkChannel.Gm);
                    lifetime.StopApplication();
                });
                break;

            case "/revive":
                player.Revive(now, inPlace: true);
                break;

            case "/scroll" when Word(1) is { } where && Cities.TryGetValue(where, out var city):
                player.Teleport(city.Map, city.X, city.Y, force: true);
                break;

            case "/skill" when Number(1) is { } type && Number(2) is { } level:
                player.SetWeaponSkill((ushort)type, (byte)level);
                break;

            case "/spawn" when Monster(Word(1) ?? "") is { } type && player.Map is not null:
                var spawned = Scatter((int)(Number(2) ?? 1),
                    (x, y) => world.Spawn(type.Id, player.MapId, (ushort)x, (ushort)y, world.MonsterIds));

                player.Tell($"Spawned {spawned} {type.Name}.");
                break;

            case "/status" when Word(1) is { } name && Enum.TryParse(name, true, out PlayerStatus status):
                Toggle(status);
                break;

            case "/weather" when Number(1) is { } weather:
                player.Send(new MsgWeather { WeatherType = (WeatherType)weather, Intensity = 1 });
                break;

            case "/xp" when Number(1) is { } xp:
                player.SetXp((byte)xp);
                break;

            default:
                player.Tell(Usages.TryGetValue(args[0], out var usage) ? $"Usage: {usage}" : $"There is no {args[0]} command.");
                break;
        }

        void Toggle(PlayerStatus status)
        {
            player.Status ^= status;
            player.Show(AttributeType.Flags, (uint)player.Status);
        }

        string? Word(int index) => args.Length > index ? args[index] : null;

        uint? Number(int index) => Word(index) is { } word && uint.TryParse(word, out var value) ? value : null;

        MonsterType? Monster(string name) =>
            uint.TryParse(name, out var id) && data.MonsterTypes.TryGetValue(id, out var byId) ? byId
                : data.MonsterTypes.Values.FirstOrDefault(type => type.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        ItemType? Find(string? text, uint? quality = null)
        {
            if (text is null)
            {
                return null;
            }

            var found = uint.TryParse(text, out var id)
                ? data.ItemTypes.GetValueOrDefault(id)
                : data.ItemTypes.Values.OrderBy(type => type.Id)
                    .FirstOrDefault(type => type.Name.Equals(text, StringComparison.OrdinalIgnoreCase));

            if (quality is null || found is null)
            {
                return found;
            }

            return data.ItemTypes.GetValueOrDefault(found.Id - found.Id % 10 + quality.Value) is { } graded && graded.Name == found.Name ? graded : found;
        }

        int Scatter(int wanted, Func<int, int, bool> place) => Enumerable.Range(1, DropSpread)
            .SelectMany(ring => Ring(player.X, player.Y, ring))
            .Where(cell => place(cell.X, cell.Y))
            .Take(wanted)
            .Count();

        static IEnumerable<(int X, int Y)> Ring(int centerX, int centerY, int radius)
        {
            for (var x = centerX - radius; x <= centerX + radius; x++)
            {
                for (var y = centerY - radius; y <= centerY + radius; y++)
                {
                    if (Entity.Distance(x, y, centerX, centerY) == radius)
                    {
                        yield return (x, y);
                    }
                }
            }
        }
    }
}
