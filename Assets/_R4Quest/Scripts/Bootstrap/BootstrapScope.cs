using DataSakura.Runtime.Utilities;
using Newtonsoft.Json;
using VContainer;
using VContainer.Unity;

    public class BootstrapScope : LifetimeScope
    {
        public ApplicationSettings _applicationSettings;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_applicationSettings);
            builder.Register<GoogleSheetDataLoadingService>(Lifetime.Scoped);
            builder.Register<ConfigDataContainer>(Lifetime.Singleton);
            builder.RegisterEntryPoint<BootstrapFlow>();
        }
}