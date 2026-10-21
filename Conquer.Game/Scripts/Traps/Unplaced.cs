namespace Conquer.Game.Scripts.Traps;

public static class Unplaced
{
    [Trap(12)]
    public static void MiniMeteorEffect(Player player)
    {
        if (player.Roll(2) < 1)
        {
            player.ShowEffect("minimeteor");
        }
    }

    [Trap(51)]
    public static void FindSquama(Player player, Trap trap)
    {
        if (player.Roll(400) < 1)
        {
            if (player.Give(ItemType.DragonBall) is not null)
            {
                player.Inform("Congratulations! You found a Squama and received a Dragon Ball luckily.");
                trap.Remove();
                player.World.Announce($"Congratulations! {player.Name} found the Squama and received a Dragon Ball luckily.",
                    TalkChannel.Gm);
            }
        }
        else if (player.Roll(300) < 6)
        {
            if (player.Give(ItemType.Meteor) is not null)
            {
                player.Inform("Congratulations! You found a Squama and received a Meteor luckily.");
                trap.Remove();
                player.World.Announce($"Congratulations! {player.Name} found the Squama and received a meteor luckily.",
                    TalkChannel.Gm);
            }
        }
        else if (player.Roll(200) < 8)
        {
            if (player.Earn(10000))
            {
                player.Inform("Congratulations! You found a Squama and received 10,000 silvers.");
                trap.Remove();
            }
        }
        else if (player.Roll(100) < 15)
        {
            if (player.Earn(5000))
            {
                player.Inform("Congratulations! You found a Squama and received 5,000 silvers.");
                trap.Remove();
            }
        }
        else if (player.Earn(1000))
        {
            player.Inform("Congratulations! You found a Squama and received 1,000 silvers.");
            trap.Remove();
        }
    }

    [Trap(58)]
    public static void FindSantaKey(Player player, Trap trap)
    {
        player.ShowEffect("archer-s");

        if (player.Give(722870) is not null)
        {
            trap.Remove();
            player.Inform("Congratulations! You got a Key. You can use it to open SantaClaus`s GiftChest.");
        }
    }
}
