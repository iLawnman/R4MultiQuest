using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class RootScope : LifetimeScope
{
    public ApplicationSettings _applicationSettings;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_applicationSettings);
        builder.Register<FXManager>(Lifetime.Singleton);
        builder.Register<ConfigDataContainer>(Lifetime.Singleton);
    }
}
