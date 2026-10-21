namespace Conquer.Game.Models;

public static class Odds
{
    private const int SpreadRolls = 999;
    private const int LowTail = 90;

    extension(Random random)
    {
        public double Spread(double range)
        {
            var roll = random.Next(SpreadRolls) + 1;
            var curve = Math.Pow(Math.Sin(roll * Math.PI / 1000), 0.25) * range;

            return roll >= LowTail ? 1 + range - curve : 1 - range + curve;
        }
    }
}
