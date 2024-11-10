using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class UISceneScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<GameflowController>();
        builder.RegisterComponentInHierarchy<SplashScreensUI>();
        builder.RegisterComponentInHierarchy<InfoPanelsController>();
        builder.RegisterEntryPoint<UIScenesStarter>();
    }
}
