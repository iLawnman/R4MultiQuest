using System.Collections;
using System.IO;
using UnityEngine;

public class LoadFromExample : MonoBehaviour
{
    IEnumerator LoadFromCache()
    {
        while (!Caching.ready)
            yield return null;

        using (var www = WWW.LoadFromCacheOrDownload("https://myserver.com/myassetBundle.unity3d", 5))
        {
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log(www.error);
                yield return null;
            }
            var myLoadedAssetBundle = www.assetBundle;

            var asset = myLoadedAssetBundle.mainAsset;
        }
    }
    
    void LoadFromFile()
    {
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "myassetBundle"));
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }

        var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("MyObject");
        Instantiate(prefab);

        myLoadedAssetBundle.Unload(false);
    }
}