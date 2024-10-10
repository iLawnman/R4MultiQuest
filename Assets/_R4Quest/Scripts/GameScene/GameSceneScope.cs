using VContainer;
using VContainer.Unity;

public class GameSceneScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<GameScenesStarter>();
    }
}
