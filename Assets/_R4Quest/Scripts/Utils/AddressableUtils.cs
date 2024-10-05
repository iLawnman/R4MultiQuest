using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public static class AddressableUtils
{
    public static async Task<bool> AddressableResourceExists(string key)
    {
        var k = Addressables.LoadResourceLocationsAsync(key);
        await k.Task;
        
        bool result = k.Result.Count > 0 ? true : false;
        Debug.Log("addressable " + key +" is " + result);
        
        return result;
    }
}