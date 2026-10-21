namespace Conquer.Game.Scripting;

[AttributeUsage(AttributeTargets.Method)]
public abstract class ScriptAttribute(uint[] ids) : Attribute
{
    public uint[] Ids => ids;
}

public sealed class NpcAttribute(params uint[] ids) : ScriptAttribute(ids);

public sealed class ItemAttribute(params uint[] itemTypeIds) : ScriptAttribute(itemTypeIds);

public sealed class MonsterAttribute(params uint[] monsterTypeIds) : ScriptAttribute(monsterTypeIds);

public sealed class TrapAttribute(params uint[] trapTypeIds) : ScriptAttribute(trapTypeIds);

public sealed class MinuteAttribute(params uint[] actionIds) : ScriptAttribute(actionIds);

public sealed class TaskAttribute(params uint[] taskIds) : ScriptAttribute(taskIds);

public sealed class DestructionAttribute(params uint[] linkIds) : ScriptAttribute(linkIds);
