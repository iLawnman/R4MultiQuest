using DataSakura.Runtime.Utilities;
using Newtonsoft.Json;
using VContainer;
using VContainer.Unity;

    public class BootstrapScope : LifetimeScope
    {
        public ApplicationSettings _applicationSettings;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<LoadingService>(Lifetime.Scoped);
            builder.RegisterInstance(_applicationSettings);
            builder.Register<DataContainer>(Lifetime.Singleton);
            builder.RegisterEntryPoint<BootstrapFlow>();
        }
}