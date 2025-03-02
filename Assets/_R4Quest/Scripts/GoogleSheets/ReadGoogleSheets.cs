using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

public class ReadGoogleSheets
{
    public const char Determiner = ';';
    
    public static async UniTask<bool> CheckTableEditedAsync(string tableId, string apiKey)
    {
        //return true;
            
        //string tableId = "1-P_YElXhGf6H2NfwVIPiq8xQV55LbWsCmF_D4cc8EFw"; // Идентификатор файла Google Sheets
        //string apiKey = "AIzaSyACKRzVQ-koaSkqmdRFFEjWDkt7GbHT0IM"; // Ваш API ключ Google
        string url = $"https://www.googleapis.com/drive/v3/files/{tableId}?fields=modifiedTime&key={apiKey}";

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.ContentType = "application/json";

        try
        {
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string result = reader.ReadToEnd();
                if (PlayerPrefs.HasKey("GoogleSheetEdited") && result != PlayerPrefs.GetString("GoogleSheetEdited"))
                {
                    PlayerPrefs.SetString("GoogleSheetEdited", result);
                    Debug.Log("GS data edited at " + result + " and need update");
                    return true;
                }
                    
                Debug.Log("GS data don't changed");
                return false;
            }
        }
        catch (WebException ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }

        return false;
    }
    
    public static UniTask<List<T>> FillDataAsync<T>(string sheetUrl, string tableName) where T : new()
    {
        var taskCompletionSource = new UniTaskCompletionSource<List<T>>();

        FillData<T>(sheetUrl, tableName, data =>
        {
            taskCompletionSource.TrySetResult(data);  // Устанавливаем результат
        });

        return taskCompletionSource.Task;
    }
    
    public static void FillData<T>(string id, string gridId, Action<List<T>> calBack) where T : new()
    {
        List<Sprite> listSprites = null;
        List<Texture> textures = null;
        List<Material> listMaterials = null;
        List<GameObject> listGameObjects = null;
        List<AudioClip> listAudioClips = null;
        
        GetTable(id, gridId, arr =>
        {
            var type = typeof(T);
            List<T> lst = new List<T>();
            for (int i = 2; i < arr.Count; i++)
            {
                if (string.IsNullOrEmpty(arr[i][0]))
                    break;
                var t = new T();
                lst.Add(t);
            }

            var header = arr[0];
            for (int i = 0; i < header.Count; i++)
            {
                if (!string.IsNullOrEmpty(header[i]))
                {
                    var property = type.GetField(header[i].Replace("\r", string.Empty),
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (property != null)
                    {
                        for (int j = 0; j < lst.Count; j++)
                        {
                            if (arr[j + 2][i] == null)
                                arr[j + 2][i] = string.Empty;
                            var value = arr[j + 2][i].Replace("\r", string.Empty);
                            var x = property.FieldType;
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (property.FieldType.IsGenericType || property.FieldType.IsArray)
                                {
                                    string typeName;
                                    if (x.IsGenericType)
                                    {
                                        typeName = x.GenericTypeArguments[0].Name;
                                    }
                                    else
                                    {
                                        typeName = x.Name.Substring(0, x.Name.Length - 2);
                                    }

                                    if (typeName == "Int32")
                                    {
                                        var listValue = value.Split(Determiner);
                                        var items = listValue.Select(int.Parse);
                                        if (x.IsGenericType)
                                            property.SetValue(lst[j], items.ToList());
                                        else
                                            property.SetValue(lst[j], items.ToArray());
                                    }
                                    else if (typeName == "Float")
                                    {
                                        var listValue = value.Split(Determiner);
                                        var items = listValue.Select(float.Parse);
                                        if (x.IsGenericType)
                                            property.SetValue(lst[j], items.ToList());
                                        else
                                            property.SetValue(lst[j], items.ToArray());
                                    }
                                    else if (typeName == "Single")
                                    {
                                        var listValue = value.Split(Determiner);
                                        var items = listValue.Select(float.Parse);
                                        if (x.IsGenericType)
                                            property.SetValue(lst[j], items.ToList());
                                        else
                                            property.SetValue(lst[j], items.ToArray());
                                    }
                                    else if (typeName == "String")
                                    {
                                        var listValue = value.Split(Determiner);
                                        var items = new List<string>();
                                        foreach (var strValue in listValue)
                                            if (!string.IsNullOrEmpty(strValue))
                                                items.Add(strValue);

                                        if (x.IsGenericType)
                                            property.SetValue(lst[j], items.ToList());
                                        else
                                            property.SetValue(lst[j], items.ToArray());
                                    }
                                    else if (typeName == "Sprite")
                                    {
                                        if (listSprites == null)
                                            listSprites = GetAllSpriteAssetsAtPath("Assets");
                                        
                                        var listValue = value.Split(Determiner);
                                        var items = new List<Sprite>();
                                        foreach (var spriteName in listValue)
                                        {
                                            if (string.IsNullOrEmpty(spriteName)) continue;
                                            var sprite = listSprites.Find(s => s.name == spriteName);
                                            items.Add(sprite);
                                        }

                                        if (x.IsGenericType)
                                            property.SetValue(lst[j], items.ToList());
                                        else
                                            property.SetValue(lst[j], items.ToArray());
                                    }
                                    else if (typeName == "Material")
                                    {
                                        if (listSprites == null)
                                            listSprites = GetAllSpriteAssetsAtPath("Assets");
                                        var listValue = value.Split(Determiner);
                                        var items = new List<Material>();
                                        foreach (var materialName in listValue)
                                        {
                                            if (string.IsNullOrEmpty(materialName)) continue;
                                            var material = listMaterials.Find(s => s.name == materialName);
                                            items.Add(material);
                                        }

                                        if (x.IsGenericType)
                                            property.SetValue(lst[j], items.ToList());
                                        else
                                            property.SetValue(lst[j], items.ToArray());
                                    }
                                    else if (typeName == "Texture")
                                    {
                                        if (listSprites == null)
                                            listSprites = GetAllSpriteAssetsAtPath("Assets");
                                        var listValue = value.Split(Determiner);
                                        var items = new List<Texture>();
                                        foreach (var item in listValue)
                                        {
                                            if (string.IsNullOrEmpty(item)) continue;
                                            var targetItem = textures.Find(s => s.name == item);
                                            items.Add(targetItem);
                                        }

                                        if (x.IsGenericType)
                                            property.SetValue(lst[j], items.ToList());
                                        else
                                            property.SetValue(lst[j], items.ToArray());
                                    }
                                    else if (typeName == "GameObject")
                                    {
                                        if (listSprites == null)
                                            listSprites = GetAllSpriteAssetsAtPath("Assets");
                                        var listValue = value.Split(Determiner);
                                        var items = new List<GameObject>();
                                        foreach (var materialName in listValue)
                                        {
                                            if (string.IsNullOrEmpty(materialName)) continue;
                                            var material = listGameObjects.Find(s => s.name == materialName);
                                            items.Add(material);
                                        }

                                        if (x.IsGenericType)
                                            property.SetValue(lst[j], items.ToList());
                                        else
                                            property.SetValue(lst[j], items.ToArray());
                                    }
                                    else
                                    {
                                        Debug.Log(typeName);
                                        var childType = Type.GetType(typeName);
                                        if (childType.IsEnum)
                                        {
                                            var listValue = value.Split(Determiner);
                                            var items = listValue.Select(s =>
                                                Convert.ChangeType(Enum.Parse(childType, s), childType)).ToList();

                                            IList instance;
                                            if (x.IsGenericType)
                                            {
                                                instance = (IList) Activator.CreateInstance(property.FieldType);
                                                foreach (var item in items)
                                                {
                                                    instance.Add(item);
                                                }
                                            }
                                            else
                                            {
                                                instance = Array.CreateInstance(childType, items.Count());
                                                for (int k = 0; k < items.Count(); k++)
                                                {
                                                    instance[k] = items[k];
                                                }
                                            }

                                            if (x.IsGenericType)
                                            {
                                                property.SetValue(lst[j], instance);
                                            }
                                            else
                                            {
                                                property.SetValue(lst[j], instance as Array);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (x.IsEnum)
                                    {
                                        object objValue = Enum.Parse(property.FieldType, value);
                                        property.SetValue(lst[j], objValue);
                                    }
                                    else if (x.Name == "Vector3")
                                    {
                                        var v3Arr = value.Replace("(", string.Empty).Replace(")", string.Empty)
                                            .Split(',');
                                        Vector3 v = new Vector3(float.Parse(v3Arr[0]), float.Parse(v3Arr[1]),
                                            float.Parse(v3Arr[2]));
                                        property.SetValue(lst[j], v);
                                    }
                                    else if (x.Name == "Sprite")
                                    {
                                        if (listSprites == null)
                                            listSprites = GetAllSpriteAssetsAtPath("Assets");
                                        var obj = listSprites.Find(s => s.name == value);
                                        property.SetValue(lst[j], obj);
                                    }
                                    else if (x.Name == "Texture")
                                    {
                                        if (textures == null)
                                            textures = GetAllTextureAssetsAtPath("Assets");
                                        var obj = textures.Find(s => s.name == value);
                                        property.SetValue(lst[j], obj);
                                    }
                                    else if (x.Name == "Material")
                                    {
                                        if (listMaterials == null)
                                            listMaterials = GetAllMaterialAssetsAtPath("Assets");
                                        var obj = listMaterials.Find(s => s.name == value);
                                        property.SetValue(lst[j], obj);
                                    }
                                    else if (x.Name == "GameObject")
                                    {
                                        if (listGameObjects == null)
                                            listGameObjects = GetAllGameObjectsAssetsAtPath("Assets");
                                        var obj = listGameObjects.Find(s => s.name == value);
                                        property.SetValue(lst[j], obj);
                                    }
                                    else if (x.Name == "AudioClip")
                                    {
                                        if (listAudioClips == null)
                                            listAudioClips = GetAllAudioClipsAtPath("Assets");
                                        var obj = listAudioClips.Find(s => s.name == value);
                                        property.SetValue(lst[j], obj);
                                    }
                                    else if (x.Name == "Color")
                                    {
                                        ColorUtility.TryParseHtmlString(value, out var color);
                                        property.SetValue(lst[j], color);
                                    }
                                    else
                                    {
                                        // Debug.Log("value:" + value + " - " + property.Name);
                                        object objValue = Convert.ChangeType(value, property.FieldType);
                                        property.SetValue(lst[j], objValue);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            calBack(lst);
        });
    }

    public static void GetTable(string id, string gridId, Action<List<List<string>>> callBack)
    {
        LoadWebClient(id, gridId, s =>
        {
            var data = SplitCsvGrid(s);
            int numRow = data.GetLength(0);
            int numCol = data.GetLength(1);

            List<List<string>> listStr = new List<List<string>>();
            for (int i = 0; i < numCol - 1; i++)
            {
                var line = new List<string>();
                for (int j = 0; j < numRow - 1; j++)
                    line.Add(data[j, i]);
                listStr.Add(line);
            }
            callBack(listStr);
        });
    }

    private static void LoadWebClient(string id, string gridId, Action<string> callBack)
    {
        string url = $@"https://docs.google.com/spreadsheet/ccc?key={id}&usp=sharing&output=csv&id=KEY&gid={gridId}";
        LoadWebClient3(url, callBack);
    }

    public static void LoadWebClient(string url, Action<string> callBack)
    {
        LoadWebClient3(url, callBack);
    }
    private static async UniTask LoadWebClient3(string id, Action<string> callBack)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(id))
        {
            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                await UniTask.Yield(); // Ожидание завершения запроса
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                string sheetData = request.downloadHandler.text;
                //Debug.Log("sheet " + sheetData);
                callBack.Invoke(sheetData);
            }
            else
            {
                Debug.LogError("Ошибка при загрузке данных с Google Sheet: " + request.error);
            }
        }
            // WWW w = new WWW(id);
            // while (!w.isDone)
            //     w.MoveNext();
            // //Debug.Log(w.text);
            // callBack(w.text);
    }

    // splits a CSV file into a 2D string array
    public static string[,] SplitCsvGrid(string csvText)
    {
        string[] lines = SmartSplit(csvText);
        int width = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string[] row = SplitCsvLine(lines[i]);
            width = Mathf.Max(width, row.Length);
        }

        string[,] outputGrid = new string[width + 1, lines.Length + 1];
        for (int y = 0; y < lines.Length; y++)
        {
            string[] row = SplitCsvLine(lines[y]);
            for (int x = 0; x < row.Length; x++)
            {
                outputGrid[x, y] = row[x];
                outputGrid[x, y] = outputGrid[x, y].Replace("\"\"", "\"");
            }
        }
        return outputGrid;
    }

    static string[] SplitCsvLine(string line)
    {
        line = line.Replace(", ", "*");
        var parts = line.Split(",");
        
        List<string> retrn = new List<string>();
        parts.ToList().ForEach(x =>
        {
            var y = x.Replace("*", ", ");
            retrn.Add(y);
        });
        return retrn.ToArray();
        
        // return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
        //         //@"(((?<x>(?=[,]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,]+)),?)",
        //         @"((""(?<x>([^""]|"""")+)""),?)",
        //         System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
        //     select m.Groups[1].Value).ToArray();
    }

    private static string[] SmartSplit(string csvString)
    {
        var inQuotes = false;
        var token = "";
        var lines = new List<string>();
        for (var i = 0; i < csvString.Length; i++)
        {
            var ch = csvString[i];
            if (inQuotes)
            {
                if (ch == '"')
                {
                    if (i < csvString.Length - 1 && csvString[i + 1] == '"')
                    {
                        i++;
                        token += '"';
                    }
                    else inQuotes = false;
                }
                else token += ch;
            }
            else
            {
                if (ch == '"') inQuotes = true;
                else if (ch == '\r')
                {
                    lines.Add(token.Substring(1, token.Length - 1));
                    token = "";
                }
                else token += ch;
            }
        }
        lines.Add(token);
        return lines.ToArray();
    }

    public static void OpenUrl(string sheetId, string gridId)
    {
        Application.OpenURL($"https://docs.google.com/spreadsheets/d/{sheetId}/edit#gid={gridId}");
    }

    private static List<Material> GetAllMaterialAssetsAtPath(string path)
    {
#if UNITY_EDITOR
        string[] paths = {path};
        var assets = UnityEditor.AssetDatabase.FindAssets("t:material", paths);
        var assetsObj = assets.Select(s =>
            UnityEditor.AssetDatabase.LoadAssetAtPath<Material>(UnityEditor.AssetDatabase.GUIDToAssetPath(s))).ToList();
        return assetsObj;
#endif
        return null;
    }

    private static List<GameObject> GetAllGameObjectsAssetsAtPath(string path)
    {
#if UNITY_EDITOR
        string[] paths = {path};
        var assets = UnityEditor.AssetDatabase.FindAssets("t:GameObject", paths);
        var assetsObj = assets.Select(s =>
                UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(UnityEditor.AssetDatabase.GUIDToAssetPath(s)))
            .ToList();
        return assetsObj;
#endif
        return null;
    }

    private static List<AudioClip> GetAllAudioClipsAtPath(string path)
    {
#if UNITY_EDITOR
        string[] paths = {path};
        var assets = UnityEditor.AssetDatabase.FindAssets("t:AudioClip", paths);
        var assetsObj = assets.Select(s =>
                UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>(UnityEditor.AssetDatabase.GUIDToAssetPath(s)))
            .ToList();
        return assetsObj;
#endif
        return null;
    }

    private static List<Sprite> GetAllSpriteAssetsAtPath(string path)
    {
#if UNITY_EDITOR
        string[] paths = {path};
        var assets = UnityEditor.AssetDatabase.FindAssets("t:sprite", paths);
        var assetsObj = assets.Select(s =>
            UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(UnityEditor.AssetDatabase.GUIDToAssetPath(s))).ToList();
        return assetsObj;
#endif
        return null;
    }

    private static List<Texture> GetAllTextureAssetsAtPath(string path)
    {
#if UNITY_EDITOR
        string[] paths = {path};
        var assets = UnityEditor.AssetDatabase.FindAssets("t:texture", paths);
        var assetsObj = assets.Select(s =>
            UnityEditor.AssetDatabase.LoadAssetAtPath<Texture>(UnityEditor.AssetDatabase.GUIDToAssetPath(s))).ToList();
        return assetsObj;
#endif
        return null;
    }

    public static void SetDirty(Object obj)
    {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(obj);
#endif
    }
}