using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using VContainer;

public class QuestSelectorUI : MonoBehaviour
{
    [SerializeField] private List<ApplicationSettings> _applicationSettings;
    [SerializeField] private GameObject buttons;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private BootstrapUI bootstrap;
    private bool menuDone;

    void Start()
    {
        bootstrap = FindObjectOfType<BootstrapUI>(includeInactive: true);
        BootstrapActions.AddApplicationToList += AddApplication;
        closeButton.onClick.AddListener(MakeMenu);

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
            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                await UniTask.Yield(); // Ожидание завершения запроса
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                string sheetData = request.downloadHandler.text;
                Debug.Log("data " + sheetData); // Обработка полученных данных
                
                string[] lines = sheetData.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                for (var index = 1; index < lines.Length; index++)
                {
                    var line = lines[index];
                    string[] parts = line.Split(',');
                    BootstrapActions.AddApplicationToList.Invoke(line);
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
        foreach (var application in _applicationSettings)
        {
            var button = Instantiate(buttonPrefab, buttons.transform);

            var tmpTxt = button.GetComponentInChildren<TMP_Text>();
            tmpTxt.text = application.applicationName;
            button.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                OnSelect(tmpTxt.text);
                menuDone = true;
            });
        }
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
        
        _applicationSettings.Add(app);

        closeButton.interactable = true;
    }

    private void OnSelect(string tmpTxt)
    {
        if(menuDone)
            return;
        
        bootstrap.gameObject.SetActive(true);
        var setting = _applicationSettings.FirstOrDefault(x => x.applicationName.Contains(tmpTxt));
        BootstrapActions.OnSelectApplication?.Invoke(setting);
        gameObject.SetActive(false);
    }
}
