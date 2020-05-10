namespace pacman.EnemiesSeenStrategies
{
    using System.Threading;
    using System;
    using System.Linq;

    public class YappyDogStrategy : IEnemiesSeenStrategy
    {
        public NextAction Next(GameGrid gameGrid, Pac pac, CancellationToken cancellation, params Pac[] enemies)
        {
            if (!enemies.Any())
            {
                return new NoAction(pac);
            }

            var closestY = enemies.OrderBy(p => Math.Abs(p.Location.X - pac.Location.X)).First();
            var closestX = enemies.OrderBy(p => Math.Abs(p.Location.Y - pac.Location.Y)).First();
            Pac closestEnemy;

            if (Math.Abs(closestX.Location.X - pac.Location.X) < Math.Abs(closestY.Location.Y - pac.Location.Y))
            {
                closestEnemy = closestX;
            }
            else
            {
                closestEnemy = closestX;
            }

            var typeToBeatEnemy = PacType.ToBeat(closestEnemy.Type);

            if (pac.SpecialActionReady && pac.Type != typeToBeatEnemy)
            {
                Console.Error.WriteLine($"Me {pac.Id} {pac.Type} switching to {typeToBeatEnemy}");
                return new SwitchAction(pac, typeToBeatEnemy);
            }

            if (pac.Type.Play(closestEnemy.Type) == PacType.Outcome.Win)
            {
                return new MoveAction(pac, closestEnemy.Location);
            }

            return new NoAction(pac);
        }
    }
}