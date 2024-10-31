using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class RootScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<FXManager>(Lifetime.Singleton);
        builder.Register<FileSyncService>(Lifetime.Singleton);
        builder.Register<ConfigDataContainer>(Lifetime.Singleton);
    }
}
