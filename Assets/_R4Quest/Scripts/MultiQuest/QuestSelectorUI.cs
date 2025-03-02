using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class QuestSelectorUI : MonoBehaviour, IUISkin
{
    [SerializeField] private List<ApplicationSettings> _applicationSettings;
    [SerializeField] private GameObject buttons;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private BootstrapUI bootstrap;
    [SerializeField] private GameObject splashScreen;
    [SerializeField] private GameObject UIRoot;
    [SerializeField] private GameObject selectorMenu;
    private bool menuSelect;
    private bool menuDone;

    void Start()
    {
        bootstrap = FindObjectOfType<BootstrapUI>(includeInactive: true);
        closeButton.onClick.AddListener(MakeMenu);

        selectorMenu.SetActive(false);

        CheckMultiQuestTable();
    }
    
    private async UniTask CheckMultiQuestTable()
    {
        string id = "1r126nBWT0kMIyZIJONteKrGTDidYMU867MbnnAR19-0"; // Идентификатор файла Google Sheets
        string apiKey = "AIzaSyACKRzVQ-koaSkqmdRFFEjWDkt7GbHT0IM"; // Ваш API ключ Google
        //string url = $"https://www.googleapis.com/drive/v3/files/{fileId}?fields=modifiedTime&key={apiKey}";
        string gridId = "0";
        string url = $@"https://docs.google.com/spreadsheet/ccc?key={id}&usp=sharing&output=csv&id=KEY&gid={gridId}";


        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            var operation = request.SendWebRequest().ToUniTask();
            await operation;

            if (request.result == UnityWebRequest.Result.Success)
            {
                string sheetData = request.downloadHandler.text;
                Debug.Log("data " + sheetData); // Обработка полученных данных
                
                string[] lines = sheetData.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                for (var index = 1; index < lines.Length; index++)
                {
                    var line = lines[index];
                    string[] parts = line.Split(',');
                    AddApplication(line);
                }
            }
            else
            {
                Debug.LogError("Ошибка при загрузке данных с Google Sheet: " + request.error);
            }
        }
    }
    private void MakeMenu()
    {
        if(menuDone)
            return;
        
        foreach (var application in _applicationSettings)
        {
            var button = Instantiate(buttonPrefab, buttons.transform);

            var tmpTxt = button.GetComponentInChildren<TMP_Text>();
            tmpTxt.text = application.applicationName;
            button.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                OnSelect(tmpTxt.text);
            });
        }

        menuDone = true;
        
        splashScreen.SetActive(false);
        UIRoot.SetActive(false);
        selectorMenu.SetActive(true);
    }

    private void AddApplication(string appString)
    {
        string[] parts = appString.Split(',');
        ApplicationSettings app = ScriptableObject.CreateInstance<ApplicationSettings>();
        app.name = parts[1];
        app.applicationName = parts[0];
        app.AddressableKey = parts[1];
        app.GoogleSheet = parts[2];
        app.GoogleSheetQuestTable = parts[3];
        app.GoogleSheetAnswersTable = parts[4];
        app.GoogleSheetResourcesTable = parts[5];
        app.GoogleSheetSplashTable = parts[7];
        app.GoogleSheetIntroTable = parts[8];
        app.GoogleSheetOutroTable = parts[9];
        app.GoogleSheetBasePrefab = parts[10];
        app.WaitConcreteNextQuest = "tgid0000";
        
        _applicationSettings.Add(app);

        closeButton.interactable = true;
    }

    private void OnSelect(string tmpTxt)
    {
        if(menuSelect)
            return;
        
        menuSelect = true;
        bootstrap.gameObject.SetActive(true);
        var setting = _applicationSettings.FirstOrDefault(x => x.applicationName.Contains(tmpTxt));
        
        BootstrapActions.OnSelectApplication?.Invoke(setting);
        gameObject.SetActive(false);
    }

    public void SetSkin(UISkin uiSkin)
    {
        throw new NotImplementedException();
    }
}
