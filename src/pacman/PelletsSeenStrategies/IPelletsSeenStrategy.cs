namespace pacman.PelletsSeenStrategies
{
    using System.Collections.Generic;
    using System.Threading;
    public interface IPelletsSeenStrategy
    {
        NextAction Next(GameGrid gameGrid,
            Pac pac,
            CancellationToken cancellation,
            List<Pellet> visiblePellets);
    }
}