using _R4Quest.Scripts.FXManager;
using VContainer;
using VContainer.Unity;

public class UISceneScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<UIScenesStarter>();
        builder.Register<CacheService>(Lifetime.Scoped);
        builder.Register<LuaScriptService>(Lifetime.Scoped);
        builder.RegisterComponentInHierarchy<SplashScreensUI>();
        builder.RegisterComponentInHierarchy<InfoPanelsController>();
        
        //builder.RegisterFactory<

        // Регистрация менеджера времени жизни
        builder.Register<IFxLifetimeManager, FxLifetimeManager>(Lifetime.Singleton);
    }
}
