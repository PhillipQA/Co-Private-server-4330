namespace Conquer.Game.Scripting;

public sealed class Dialog(Player player, Entity? npc = null, Item? item = null)
{
    private const byte Close = 255;
    private const int MaxTextLength = 250;

    private readonly uint _id = npc?.Id ?? 0;

    private TaskCompletionSource<(byte Option, string Input)> _answer = new();
    private TaskCompletionSource<MsgNpcInfo>? _placement;
    private Map? _placementMap;

    public ushort Avatar { get; set; } = 1;

    public async Task<int> Ask(string text, params string[] options)
    {
        Text(text);
        Links(options);
        return (await ShowAsync()).Option - 1;
    }

    public async Task<(int Option, string Input)> Ask(string text, string prompt, byte maxLength, params string[] options)
    {
        Text(text);
        Links(options);
        Edit(prompt, (byte)(options.Length + 1), maxLength);
        var (option, input) = await ShowAsync();
        return (option - 1, input);
    }

    public async Task Confirm(string text, params string[] options)
    {
        if (await Ask(text, options) != 0)
        {
            throw new OperationCanceledException();
        }
    }

    public void Require(bool condition)
    {
        if (!condition)
        {
            player.Inform("Error: Failed to meet condition.");
            throw new OperationCanceledException();
        }
    }

    public void Say(string text, params string[] options)
    {
        Text(text);

        foreach (var option in options)
        {
            Send(DialogType.Link, option, Close);
        }

        ShowAsync();
    }

    public async Task<string> Input(string text, string prompt, byte maxLength = 16, string? cancel = "Cancel")
    {
        Text(text);
        Edit(prompt, 1, maxLength);

        if (cancel is not null)
        {
            Send(DialogType.Link, cancel, Close);
        }

        return (await ShowAsync()).Input;
    }

    public Task PickSpot(ushort type, uint look, uint region = 0)
    {
        _placementMap = player.Map;
        player.Send(new MsgNpc { Id = region, Data = look, Action = NpcAction.LayNpc, NpcType = type });
        return (_placement = new()).Task;
    }

    public void Later(uint seconds, Func<Task> chain) =>
        player.World.Scripts.Later(player, this, TimeSpan.FromSeconds(seconds), chain);

    public void Later(uint seconds, Action chain) => Later(seconds, () =>
    {
        chain();
        return Task.CompletedTask;
    });

    public bool Place(DynaNpc laid, uint region = 0) => _placement?.Task is { IsCompletedSuccessfully: true, Result: var spot } &&
                                                      player.PlaceNpc(laid, spot.X, spot.Y, spot.Look % 10, region, spot.NpcType,
                                                          (byte)spot.NpcSort);

    internal void Answer(byte option, string input)
    {
        if (option == Close || npc is not null && !player.CanSee(npc))
        {
            Cancel();
            return;
        }

        _answer.TrySetResult((option, input));
    }

    internal void SpotPicked(MsgNpcInfo spot)
    {
        if (item is not null && player.FindItem(item.Id) is null || player.Map != _placementMap)
        {
            Cancel();
            return;
        }

        _placement?.TrySetResult(spot);
    }

    internal void Cancel()
    {
        _answer.TrySetCanceled();
        _placement?.TrySetCanceled();
    }

    private void Text(string text)
    {
        foreach (var line in text.Split('\n'))
        {
            var rest = line;

            while (rest.Length > MaxTextLength)
            {
                var cut = rest.LastIndexOf(' ', MaxTextLength - 1) is > 0 and var space ? space + 1 : MaxTextLength;
                Send(DialogType.Text, rest[..cut]);
                rest = rest[cut..];
            }

            Send(DialogType.Text, rest);
        }
    }

    private void Links(string[] options)
    {
        for (var i = 0; i < options.Length; i++)
        {
            Send(DialogType.Link, options[i], (byte)(i + 1));
        }
    }

    private void Edit(string prompt, byte option, byte maxLength) => player.Send(new MsgTaskDialog
    {
        Id = _id,
        DialogType = DialogType.Edit,
        Option = option,
        Data = maxLength,
        Strings = [prompt]
    });

    private Task<(byte Option, string Input)> ShowAsync()
    {
        if (player.Dialog != this)
        {
            player.Dialog?.Cancel();
            player.Dialog = this;
        }

        if (Avatar != 0)
        {
            player.Send(new MsgTaskDialog { Id = _id, DialogType = DialogType.Picture, Data = Avatar });
        }

        player.Send(new MsgTaskDialog { Id = _id, DialogType = DialogType.Create, Option = Close });
        return (_answer = new()).Task;
    }

    private void Send(DialogType type, string text, byte option = 0) =>
        player.Send(new MsgTaskDialog { Id = _id, DialogType = type, Option = option, Strings = [text] });
}
