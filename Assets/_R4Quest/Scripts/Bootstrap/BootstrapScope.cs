using VContainer;
using VContainer.Unity;

    public class BootstrapScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<GoogleSheetDataLoadingService>(Lifetime.Scoped);
            builder.RegisterEntryPoint<BootstrapFlow>();
            builder.RegisterComponentInHierarchy<QuestSelectorUI>();
        }
}