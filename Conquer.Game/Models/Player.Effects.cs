using System.Collections.Frozen;

namespace Conquer.Game.Models;

public partial class Player
{
    private const int EffectChance = 10;
    private const int MaxFireworksText = 8;

    private static readonly FrozenDictionary<Gem, string> GemEffects = new Dictionary<Gem, string>
    {
        [Gem.SuperPhoenixGem] = "phoenix",
        [Gem.SuperDragonGem] = "goldendragon",
        [Gem.SuperFuryGem] = "fastflash",
        [Gem.SuperRainbowGem] = "rainbow",
        [Gem.SuperKylinGem] = "goldenkylin",
        [Gem.SuperVioletGem] = "purpleray",
        [Gem.SuperMoonGem] = "moon"
    }.ToFrozenDictionary();

    private static readonly FrozenDictionary<byte, string> BlessEffects = new Dictionary<byte, string>
    {
        [1] = "Aegis1",
        [3] = "Aegis2",
        [5] = "Aegis5",
        [7] = "Aegis7"
    }.ToFrozenDictionary();

    public void ShowEffect(string effect) =>
        Map?.Broadcast(this, new MsgName { Id = Id, Action = NameAction.RoleEffect, Strings = [effect] });

    public void PlaySound(string sound) => Send(new MsgName { Action = NameAction.Sound, Strings = [sound] });

    public void Fireworks() => Map?.Broadcast(this, new MsgItem { Id = Id, Action = ItemAction.Fireworks });

    public bool Fireworks(string text)
    {
        if (text.Length is 0 or > MaxFireworksText)
        {
            return false;
        }

        Map?.Broadcast(this, new MsgName { Action = NameAction.Fireworks, Strings = [text] });
        return true;
    }

    private void Sparkle()
    {
        var gems = Equipment.SelectMany(item => new[] { item.Gem1, item.Gem2 }).Where(gem => gem is not (Gem.None or Gem.Empty)).ToList();

        if (gems.Count > 0 && World.Random.Next(EffectChance) == 0 &&
            GemEffects.TryGetValue(gems[World.Random.Next(gems.Count)], out var effect))
        {
            ShowEffect(effect);
        }
    }

    private void Shine()
    {
        var effects = Equipment.Select(item => BlessEffects.GetValueOrDefault(item.Bless)).OfType<string>().Distinct().ToList();

        if (effects.Count > 0 && World.Random.Next(EffectChance) == 0)
        {
            ShowEffect(effects[World.Random.Next(effects.Count)]);
        }
    }
}
