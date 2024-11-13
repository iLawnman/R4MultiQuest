using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "AppSettings", menuName = "iLawn/AppSettings")]
public class ApplicationSettings : ScriptableObject
{
    public string applicationName;
    public string companyName;
    public string applicationVersion;
    public string applicationBuild;
    public string bundleId;
    public string category;
    public string icon;
    public string GoogleSheet;
    public string GoogleSheetQuestTable;
    public string GoogleSheetAnswersTable;
    public string GoogleSheetResourcesTable;
    public string RemoteSettings;
    public string GoogleSheetSplashTable;
    public string GoogleSheetIntroTable;
    public string GoogleSheetOutroTable;
    public string AddressableKey;
    public string GoogleSheetBasePrefab;
}
