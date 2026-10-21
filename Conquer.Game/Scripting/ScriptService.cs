using System.Reflection;
using System.Reflection.Metadata;

[assembly: MetadataUpdateHandler(typeof(ScriptService))]

namespace Conquer.Game.Scripting;

public class ScriptService
{
    private static int _version;

    private readonly ILogger<ScriptService> _logger;
    private readonly IServiceProvider _services;
    private int _loaded;
    private Dictionary<(Type, uint), BoundHandler> _scripts = [];
    private List<(string Name, BoundHandler Script)> _minutes = [];

    public ScriptService(IServiceProvider services, ILogger<ScriptService> logger)
    {
        _services = services;
        _logger = logger;
        Load();
    }

    public static void UpdateApplication(Type[]? _) => _version++;

    public void OnTalk(Player player, Entity npc)
    {
        if ((Find<NpcAttribute>(npc.Id) ?? (npc is DynaNpc { Task0: > 0 } laid ? Find<TaskAttribute>(laid.Task0) : null)) is { } script)
        {
            _ = RunAsync(script, player, Open(player, npc), npc);
        }
    }

    public bool OnUse(Player player, Item item)
    {
        if (Find<ItemAttribute>(item.ItemTypeId) is not { } script)
        {
            return false;
        }

        _ = RunAsync(script, player, Open(player, item: item), null, item);
        return true;
    }

    public void OnKill(Player killer, Monster monster)
    {
        if (Find<MonsterAttribute>(monster.MonsterType.Id) is { } script)
        {
            _ = RunAsync(script, killer, null, monster);
        }
    }

    public bool OnDestroy(Player killer, DynaNpc npc)
    {
        if (npc.LinkId == 0 || Find<DestructionAttribute>(npc.LinkId) is not { } script)
        {
            return false;
        }

        _ = RunAsync(script, killer, null, npc);
        return true;
    }

    public void OnStep(Player player, Trap trap)
    {
        if (Find<TrapAttribute>(trap.Type) is { } script)
        {
            _ = RunAsync(script, player, null, trap);
        }
    }

    public void Tick()
    {
        Refresh();

        foreach (var (name, script) in _minutes)
        {
            _ = RunAsync(name, script);
        }
    }

    internal void Later(Player player, Dialog dialog, TimeSpan delay, Func<Task> chain) =>
        player.World.After(delay, () =>
        {
            if (player.World.Players.GetValueOrDefault(player.Id) == player)
            {
                _ = RunAsync(chain, player, dialog);
            }
        });

    private static Dialog Open(Player player, Entity? npc = null, Item? item = null)
    {
        player.Dialog?.Cancel();
        return player.Dialog = new(player, npc, item);
    }

    private async Task RunAsync(string name, BoundHandler script)
    {
        try
        {
            await script.InvokeAsync(_services, null!, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Minute script {Script} failed.", name);
        }
    }

    private Task RunAsync(BoundHandler script, Player player, Dialog? dialog, Entity? subject, Item? item = null) =>
        RunAsync(() => script.InvokeAsync(_services, player.Client, null, dialog, subject, item), player, dialog);

    private async Task RunAsync(Func<Task> work, Player player, Dialog? dialog)
    {
        try
        {
            await work();
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Script failed for {Player}.", player.Name);
        }

        if (dialog is not null && player.Dialog == dialog)
        {
            player.Dialog = null;
        }
    }

    private BoundHandler? Find<T>(uint id) where T : ScriptAttribute
    {
        Refresh();
        return _scripts.GetValueOrDefault((typeof(T), id));
    }

    private void Refresh()
    {
        if (_loaded != _version)
        {
            _loaded = _version;
            Load();
        }
    }

    private void Load()
    {
        var isService = _services.GetRequiredService<IServiceProviderIsService>();

        var scripts = typeof(ScriptService).Assembly.GetTypes()
            .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Static))
            .SelectMany(method => method.GetCustomAttributes<ScriptAttribute>()
                .SelectMany(attribute => attribute.Ids.Select(id => (Type: attribute.GetType(), Id: id, Method: method,
                    Script: new BoundHandler(method, typeof(GameClient), isService, typeof(Dialog), typeof(Entity), typeof(Item))))))
            .ToList();

        _scripts = scripts.ToDictionary(script => (script.Type, script.Id), script => script.Script);
        _minutes = [..scripts.Where(script => script.Type == typeof(MinuteAttribute))
            .Select(script => ($"{script.Method.DeclaringType!.Name}.{script.Method.Name}", script.Script))];
        _logger.LogInformation("Loaded {Count} scripts.", _scripts.Count);
    }
}
