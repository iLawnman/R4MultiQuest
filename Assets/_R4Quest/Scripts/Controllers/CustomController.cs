using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;

public class CustomController : Controller
{
    private readonly IFactory<Custom2Controller> _barControllerFactoy;
	
    public CustomController(IFactory<Custom2Controller> barControllerFactoy)
    {
        _barControllerFactoy = barControllerFactoy;
    }
	
    protected override UniTask OnInitialize(CancellationToken token)
    {
        return UniTask.CompletedTask;
    }

    protected override async UniTask Running(CancellationToken token)
    {
        //await Metadata.Children.Run(_barControllerFactoy);
    }

    protected override UniTask OnStop(bool isForced)
    {
        throw new System.NotImplementedException();
    }

    protected override bool HandleEvent(ControllerEvent e)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnDispose(bool disposing)
    {
        throw new System.NotImplementedException();
    }
}

public class Custom2Controller : Controller
{
    protected override UniTask OnInitialize(CancellationToken token)
    {
        throw new System.NotImplementedException();
    }

    protected override UniTask Running(CancellationToken token)
    {
        throw new System.NotImplementedException();
    }

    protected override UniTask OnStop(bool isForced)
    {
        throw new System.NotImplementedException();
    }

    protected override bool HandleEvent(ControllerEvent e)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnDispose(bool disposing)
    {
        throw new System.NotImplementedException();
    }
}

public interface IFactory<T>
{
}