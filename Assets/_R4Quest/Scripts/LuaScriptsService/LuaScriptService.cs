using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MoonSharp.Interpreter;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class LuaScriptService : IStartable 
{
    [Inject] private CacheService cacheService;
    Dictionary<string, string> scripts = new Dictionary<string, string>();

    public void Start()
    {
        Debug.Log("start lua service ");
        RegisterAllScripts();
    }
    void RegisterAllScripts()
    {
        var _load = cacheService.LoadCachedObject("CallQuestStart.lua");
        string a = Convert.ToString(_load);
        Debug.Log("register all lua " + a);

        scripts.Clear();
        List<string> list = new List<string>(){a};
        foreach (string _ta in list)
        {
            Debug.Log("register " + _ta);
            TextAsset ta = new TextAsset(_ta);
            scripts.Add("CallQuestStart.lua", ta.text);
        }
        Script.DefaultOptions.ScriptLoader = new MoonSharp.Interpreter.Loaders.UnityAssetsScriptLoader(scripts);
    }

    public void ExecuteAction(string scriptName, string functionName, string input)
    {
        Script script = new Script();
        var scr = scripts[scriptName];
        Debug.Log("scr " + scr);
        script.DoString(scr);
        DynValue result = script.Call(script.Globals[functionName], input);
        
        Debug.Log($"Result from {scriptName}.{functionName}: {result.String}");
    }
}