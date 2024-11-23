using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class LuaScriptService : IStartable 
{
    Dictionary<string, string> scripts = new Dictionary<string, string>();

    public void Start()
    {
        Debug.Log("start lua service ");
        RegisterAllScripts();
    }
    void RegisterAllScripts()
    {
        scripts.Clear();
        var _loadScripts = CacheService.LoadCachedObjects(".lua");
        Debug.Log("register all lua - " + _loadScripts.Count);

        foreach (var script in _loadScripts)
        {
            var s = System.Text.Encoding.UTF8.GetString(script.Value);
            string a = Convert.ToString(s);
            TextAsset ta = new TextAsset(a);
            scripts.Add(script.Key, ta.text);
            Debug.Log("register " + a);
        }
        Script.DefaultOptions.ScriptLoader = new MoonSharp.Interpreter.Loaders.UnityAssetsScriptLoader(scripts);
    }

    public void ExecuteAction(string scriptName, string functionName, string input)
    {
        if (!scripts.ContainsKey(scriptName))
        {
            Debug.LogWarning("no lua script registered");
            return;
        }

        Script script = new Script();
        var scr = scripts[scriptName];
        script.DoString(scr);
        DynValue result = script.Call(script.Globals[functionName], input);
        
        Debug.Log($"Result from {scriptName}.{functionName}: {result.String}");
    }
}