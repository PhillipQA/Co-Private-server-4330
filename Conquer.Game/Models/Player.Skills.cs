namespace Conquer.Game.Models;

public partial class Player
{
    private const int MinMasterRate = 10;

    public IEnumerable<Magic> KnownMagics => Magics.Where(magic => !magic.Unlearned);
    public IEnumerable<WeaponSkill> KnownWeaponSkills => WeaponSkills.Where(skill => !skill.Unlearned);

    public Magic? KnownMagic(ushort magicType) => KnownMagics.FirstOrDefault(magic => magic.Type == magicType);

    public bool Knows(ushort magicType, int level = -1) =>
        KnownMagics.Any(magic => magic.Type == magicType && (level < 0 || magic.Level == level));

    public bool IsSkilled(ushort weaponSkillType, int level = 0) =>
        KnownWeaponSkills.Any(skill => skill.Type == weaponSkillType && skill.Level >= level);

    public bool Learn(ushort magicType, byte level = 0)
    {
        if (Knows(magicType))
        {
            return false;
        }

        SetMagic(magicType, level);
        return true;
    }

    public void SetMagic(ushort magicType, byte level)
    {
        if (Magics.FirstOrDefault(magic => magic.Type == magicType) is not { } magic)
        {
            magic = new() { PlayerId = Id, Type = magicType };
            Magics.Add(magic);
        }

        (magic.Level, magic.Experience, magic.Unlearned) = (level, 0, false);
        Send(new MsgMagicInfo(magic));
    }

    public bool LevelUpMagic(ushort magicType)
    {
        if (KnownMagic(magicType) is not { } magic || !World.Data.MagicTypes.ContainsKey((magicType, (byte)(magic.Level + 1))))
        {
            return false;
        }

        SetMagic(magicType, (byte)(magic.Level + 1));
        return true;
    }

    public void Practice(Magic magic, double amount)
    {
        var data = World.Data.MagicTypes;

        if (!data.ContainsKey((magic.Type, (ushort)(magic.Level + 1))) || !data.TryGetValue((magic.Type, magic.Level), out var spell) ||
            Level < spell.RequiredLevel)
        {
            return;
        }

        magic.Experience += (uint)(amount * (100 + Stats.MoonGems) / 100);

        if (magic.Experience < spell.RequiredExp && !Recalls(magic.Level, magic.OldLevel))
        {
            Send(new MsgFlushExp { Experience = magic.Experience, SkillType = magic.Type, Action = FlushExpAction.Magic });
            return;
        }

        (magic.Level, magic.Experience) = ((byte)(magic.Level + 1), 0);
        Send(new MsgMagicInfo(magic));
    }

    public void SetWeaponSkill(ushort weaponSkillType, byte level)
    {
        var known = SkillOf(weaponSkillType);
        (known.Level, known.Experience, known.Unlearned) = (level, 0, false);
        Send(new MsgWeaponSkill(known));
        Tell("Congrats. Your weapon proficiency is one level up.", TalkChannel.System);
    }

    public void Train(ushort weaponSkillType, double amount)
    {
        var gained = (uint)(amount * (100 + Stats.VioletGems) / 100);
        var skill = SkillOf(weaponSkillType);

        if (skill is { Level: 0 } or { Unlearned: true })
        {
            (skill.Level, skill.Experience, skill.Unlearned) = (1, gained, false);
            Send(new MsgWeaponSkill(skill));
            return;
        }

        if (!World.Data.WeaponSkillExps.TryGetValue(skill.Level, out var next))
        {
            return;
        }

        skill.Experience += skill.Level > MasterSkillLevel
            ? gained * (uint)Math.Max(MinMasterRate, 100 - (skill.Level - MasterSkillLevel) * 20) / 100 / 2
            : gained;

        if (skill.Experience <= next.Experience)
        {
            Send(new MsgFlushExp { Experience = skill.Experience, SkillType = skill.Type, Action = FlushExpAction.WeaponSkill });
            return;
        }

        var level = (byte)(skill.Level + 1);
        (skill.Level, skill.Experience, skill.OldLevel) = Recalls(level, skill.OldLevel)
            ? (skill.OldLevel, 0u, (byte)0)
            : (level, 0u, skill.OldLevel);
        Send(new MsgWeaponSkill(skill));
        Tell("Congratulations! Your weapon proficiency has been improved.", TalkChannel.System);
    }

    public void ResetMagics(params ushort[] types)
    {
        foreach (var magic in Magics.Where(magic => types.Contains(magic.Type)))
        {
            magic.OldLevel = magic.Level;
            (magic.Level, magic.Experience, magic.Unlearned) = (0, 0, false);
            Send(new MsgMagicInfo(magic));
        }
    }

    public void RemoveMagics(params ushort[] types)
    {
        foreach (var magic in Magics.Where(magic => types.Contains(magic.Type)).ToList())
        {
            Magics.Remove(magic);
            Send(new MsgAction { Id = Id, Data = magic.Type, Action = ActionType.DropMagic });
        }
    }

    public void UnlearnMagics(params ushort[] types)
    {
        foreach (var magic in KnownMagics.Where(magic => types.Contains(magic.Type)).ToList())
        {
            (magic.OldLevel, magic.Unlearned) = (magic.Level, true);
            Send(new MsgAction { Id = Id, Data = magic.Type, Action = ActionType.DropMagic });
        }
    }

    public void UnlearnWeaponSkills()
    {
        foreach (var skill in KnownWeaponSkills.ToList())
        {
            (skill.OldLevel, skill.Unlearned) = (skill.Level, true);
            Send(new MsgAction { Id = Id, Data = skill.Type, Action = ActionType.DropSkill });
        }
    }

    private WeaponSkill SkillOf(ushort weaponSkillType)
    {
        if (WeaponSkills.FirstOrDefault(skill => skill.Type == weaponSkillType) is { } known)
        {
            return known;
        }

        known = new() { PlayerId = Id, Type = weaponSkillType };
        WeaponSkills.Add(known);
        return known;
    }

    private static bool Recalls(byte level, byte oldLevel) => level >= oldLevel / 2 && level < oldLevel;
}
