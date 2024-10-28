using System;
using System.Threading;
using Cysharp.Threading.Tasks;

public class BaseController : Controller
{
    public BaseController()
    {
    }

    protected override UniTask OnInitialize(CancellationToken token)
    {
        return UniTask.CompletedTask;
    }

    protected override UniTask Running(CancellationToken token)
    {
        return UniTask.CompletedTask;
    }

    protected override UniTask OnStop(bool isForced)
    {
        return UniTask.CompletedTask;
    }

    protected override bool HandleEvent(ControllerEvent e)
    {
        return false;
    }

    protected override void OnDispose(bool disposing)
    {
        
    }
}

public class ControllerEvent
{
}

public abstract class Controller
{
    protected abstract UniTask OnInitialize(CancellationToken token);

    protected abstract UniTask Running(CancellationToken token);

    protected abstract UniTask OnStop(bool isForced);
    
    protected abstract bool HandleEvent(ControllerEvent e);

    protected abstract void OnDispose(bool disposing);
}