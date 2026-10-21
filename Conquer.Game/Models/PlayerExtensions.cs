namespace Conquer.Game.Models;

public static class PlayerExtensions
{
    extension(IEnumerable<Player> players)
    {
        public void Send(IMessage message)
        {
            foreach (var player in players)
            {
                player.Send(message);
            }
        }
    }
}
