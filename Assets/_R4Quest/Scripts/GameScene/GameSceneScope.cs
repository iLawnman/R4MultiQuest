using VContainer;
using VContainer.Unity;

public class GameSceneScope : RootScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<GameScenesStarter>();
    }
}
