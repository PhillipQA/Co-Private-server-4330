namespace Conquer.Game.Models;

public partial class Player
{
    public bool CanEarn(uint amount) => Money + (ulong)amount <= MoneyLimit;

    public bool Spend(uint amount)
    {
        if (Money < amount)
        {
            return false;
        }

        Money -= amount;
        Update(AttributeType.Money, Money);
        return true;
    }

    public bool Earn(uint amount)
    {
        if (!CanEarn(amount))
        {
            return false;
        }

        Money += amount;
        Update(AttributeType.Money, Money);
        return true;
    }

    public void Pay(Player other, uint amount)
    {
        if (amount > 0 && other.CanEarn(amount) && Spend(amount))
        {
            other.Earn(amount);
        }
    }

    public bool SpendVirtue(uint amount)
    {
        if (Virtue < amount)
        {
            return false;
        }

        Virtue = Virtue - amount;
        return true;
    }

    public void EarnVirtue(uint amount) => Virtue = Virtue + amount;
}
