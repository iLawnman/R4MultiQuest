using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

public class GoogleSheetDataLoadingService
{
    [Inject] private ApplicationSettings _applicationSettings;
    
    public async UniTask Loading(ConfigDataContainer configDataContainer)
    {
        // if(!AddressableUtils.AddressableResourceExists(_applicationSettings.AddressableKey).Result)
        //     return;

        await Addressables.InitializeAsync();
        BootstrapActions.OnShowInfo?.Invoke("Loading Dependencies");
        Debug.Log("loading dependencies");
        var handle = Addressables.DownloadDependenciesAsync(_applicationSettings.AddressableKey);

        while (!handle.IsDone)
        {
            await UniTask.Yield();
        }
        
        BootstrapActions.OnShowInfo?.Invoke("Loading Data");
        Debug.Log("loading data");
        
        bool questReady = false;
        bool answerReady = false;
        bool resourcesReady = false;
        
        ReadGoogleSheets.FillData<QuestData>(_applicationSettings.GoogleSheet,
                _applicationSettings.GoogleSheetQuestTable,
                list =>
                {
                    configDataContainer.ApplicationData.Quests = list;
                    questReady = true;
                });
        ReadGoogleSheets.FillData<AnswersData>(_applicationSettings.GoogleSheet,
                _applicationSettings.GoogleSheetAnswersTable,
                list =>
                {
                    configDataContainer.ApplicationData.Answers = list;
                    answerReady = true;
                });
        ReadGoogleSheets.FillData<ResourcesData>(_applicationSettings.GoogleSheet,
                _applicationSettings.GoogleSheetResourcesTable,
                list =>
                {
                    configDataContainer.ApplicationData.Resources = list;
                    resourcesReady = true;
                });
        
        while (!questReady && !answerReady && !resourcesReady)
        {
            await UniTask.Yield();
        }

        BootstrapActions.OnShowInfo?.Invoke("Data Loaded");
        await UniTask.Delay(2000);
        BootstrapActions.OnShowInfo?.Invoke(string.Empty);

    }
}