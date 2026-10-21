namespace Conquer.Game.Messages;

public record MsgWeaponSkill : IMessage
{
    public MsgWeaponSkill()
    {
    }

    public MsgWeaponSkill(WeaponSkill weaponSkill)
    {
        WeaponSkillType = weaponSkill.Type;
        Level = weaponSkill.Level;
        Experience = weaponSkill.Experience;
    }

    public uint WeaponSkillType { get; set; }
    public uint Level { get; set; }
    public uint Experience { get; set; }
    public ushort Size => 16;
    public MessageType Type => MessageType.MsgWeaponSkill;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        WeaponSkillType = ReadUInt32LittleEndian(buffer[4..]);
        Level = ReadUInt32LittleEndian(buffer[8..]);
        Experience = ReadUInt32LittleEndian(buffer[12..]);
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], WeaponSkillType);
        WriteUInt32LittleEndian(buffer[8..], Level);
        WriteUInt32LittleEndian(buffer[12..], Experience);
    }
}