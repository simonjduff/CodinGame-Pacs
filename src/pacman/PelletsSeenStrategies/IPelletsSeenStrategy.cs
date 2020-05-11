namespace pacman.PelletsSeenStrategies
{
    using System.Collections.Generic;
    using System.Threading;
    public interface IPelletsSeenStrategy
    {
        NextAction Next(Pac pac,
            CancellationToken cancellation,
            List<Pellet> visiblePellets);
    }
}