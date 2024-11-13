using UnityEngine;
using VContainer.Unity;

public class UIScenesStarter : IStartable
{
    private readonly ConfigDataContainer _dataContainer;
    private readonly CacheService _cacheService;
    private readonly LuaScriptService _luaScriptService;

    public UIScenesStarter(ConfigDataContainer dataContainer, 
        CacheService cacheService, 
        LuaScriptService luaScriptService)
    {
        _dataContainer = dataContainer;
        _cacheService = cacheService;
        _luaScriptService = luaScriptService;
    }

    public void Start()
    {
        Debug.Log("start uiscene ");
        _cacheService.Start();
        _luaScriptService.Start();
    }
}