using VContainer;
using VContainer.Unity;

public class UISceneScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<UIScenesStarter>();
        //builder.RegisterComponentInHierarchy<GameflowController>();
        builder.Register<CacheService>(Lifetime.Scoped);
        builder.Register<LuaScriptService>(Lifetime.Scoped);
        builder.RegisterComponentInHierarchy<SplashScreensUI>();
        builder.RegisterComponentInHierarchy<InfoPanelsController>();
    }
}
