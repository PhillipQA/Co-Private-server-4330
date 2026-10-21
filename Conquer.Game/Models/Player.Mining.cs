using System.Collections.Frozen;

namespace Conquer.Game.Models;

public partial class Player
{
    private const ushort Pickaxe = 562;
    private const byte NoviceMiner = 15;
    private const int NovicePercent = 80;
    private const int OreRolls = 10_000;
    private const int Tiring = 200;
    private const double RichnessSpread = 0.5;

    private const uint Iron = 1072010;
    private const uint Copper = 1072020;
    private const uint Euxite = 1072031;
    private const uint Silver = 1072040;
    private const uint Gold = 1072050;
    private const uint GemBase = 700000;

    private static readonly TimeSpan Swing = TimeSpan.FromSeconds(3);

    private static readonly FrozenDictionary<uint, string> OreFound = new Dictionary<uint, string>
    {
        [Iron] = "You gained an iron ore!",
        [Copper] = "You gained copper ore!",
        [Euxite / 10 * 10] = "You gained an euxenite ore!",
        [Silver] = "You gained a silver ore!",
        [Gold] = "You gained a gold ore!"
    }.ToFrozenDictionary();

    private static readonly FrozenDictionary<byte, Lode> Lodes = new Dictionary<byte, Lode>
    {
        [1] = new(3, 20, Euxite, 1, 70, Iron, 1070,
            [new(20_000_000, 7, 37, [Gem.NormalFuryGem, Gem.NormalDragonGem, Gem.NormalPhoenixGem]), new(3_000_000, 7, 37, [Gem.NormalTortoiseGem])]),
        [2] = new(6, 17, Iron, 10, 167, Copper, 1017,
            [new(12_000_000, 12, 60, [Gem.NormalPhoenixGem, Gem.NormalRainbowGem, Gem.NormalKylinGem, Gem.NormalDragonGem])]),
        [3] = new(10, 17, Copper, 10, 167, Silver, 1017, [new(10_000_000, 12, 60, [Gem.NormalMoonGem, Gem.NormalVioletGem])])
    }.ToFrozenDictionary();

    private DateTimeOffset _swung;
    private bool _mining;
    private int _richness;
    private int _swings;

    public void Mine(DateTimeOffset now)
    {
        if (now - _swung < Swing)
        {
            return;
        }

        var starting = !_mining;
        _swung = now;
        _mining = false;

        if (!IsAlive)
        {
            Tell("You cannot mine while you are dead.");
            return;
        }

        if (Map?.Info.Is(MapFlags.MineField) != true || !Lodes.TryGetValue(Map.Level, out var lode))
        {
            Tell("You can't mine here.");
            return;
        }

        if (ItemAt(ItemPosition.Right) is not { } pick ||
            World.Data.ItemTypes.GetValueOrDefault(pick.ItemTypeId)?.WeaponSkill != Pickaxe)
        {
            Tell("You need a pickaxe to mine.");
            return;
        }

        if (starting)
        {
            _richness = (int)(World.Random.Spread(RichnessSpread) * 100);
            _swings = 0;
        }

        _mining = true;
        Map.Broadcast(this, Act(ActionType.Mine));

        foreach (var found in Dig(lode))
        {
            if (World.Data.ItemTypes.TryGetValue(found, out var type))
            {
                Unearth(type, now);
            }
        }
    }

    private void Unearth(ItemType type, DateTimeOffset now)
    {
        if (FreeSlots > 0)
        {
            Give(type);
        }
        else if (!Map!.Drop(new MapItem { Item = Item.Of(type), OwnerId = Id }, this, now))
        {
            _mining = false;
            return;
        }

        if (OreFound.GetValueOrDefault(type.Id / 10 * 10) is { } found)
        {
            Tell(found, TalkChannel.System);
        }
    }

    private List<uint> Dig(Lode lode)
    {
        var rate = _richness * Tiring / (_swings++ + Tiring) * (Level <= NoviceMiner ? NovicePercent : 100) / 100;
        var roll = OreRolls - 1 - Roll(OreRolls);
        List<uint> found = [];

        if (roll < rate * lode.GoldEdge / 100)
        {
            found.Add(Gold + (uint)Roll(lode.GoldGrades));
        }
        else if (roll < rate * lode.MiddleEdge / 100)
        {
            found.Add(lode.Middle + (uint)Roll(lode.MiddleGrades));
        }
        else if (roll < rate * lode.CommonEdge / 100)
        {
            found.Add(lode.Common + (Roll(10) < 6 ? 0u : 5u) + (uint)Roll(5));
        }

        if (Level > NoviceMiner)
        {
            found.AddRange(lode.Seams.Select(Gemstone).OfType<uint>());
        }

        return found;
    }

    private uint? Gemstone(Seam seam)
    {
        var roll = Roll(seam.Rolls);
        var grade = roll < 1 ? 3u : roll < seam.RefinedEdge ? 2u : roll < seam.NormalEdge ? 1u : 0u;

        return grade == 0 ? null : GemBase + (uint)seam.Kinds[Roll(seam.Kinds.Length)] - 1 + grade;
    }

    private sealed record Lode(
        int GoldGrades,
        int GoldEdge,
        uint Middle,
        int MiddleGrades,
        int MiddleEdge,
        uint Common,
        int CommonEdge,
        Seam[] Seams);

    private sealed record Seam(int Rolls, int RefinedEdge, int NormalEdge, Gem[] Kinds);
}
