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

        bool updated = await ReadGoogleSheets.CheckTableEditedAsync(_applicationSettings.GoogleSheet,
            _applicationSettings.GoogleSheetQuestTable);
        
        if (!updated)
        {
            Debug.Log("no need update, but temporally");
            //return;
        }

        BootstrapActions.OnShowInfo?.Invoke("Loading Dependencies");
        Debug.Log("loading dependencies");
        var handle = Addressables.DownloadDependenciesAsync(_applicationSettings.AddressableKey);

        while (!handle.IsDone)
        {
            await UniTask.Yield();
        }
        
        Debug.Log("loading data");
        BootstrapActions.OnShowInfo?.Invoke("Loading Quests");
        configDataContainer.ApplicationData.Quests = await ReadGoogleSheets.FillDataAsync<QuestData>(_applicationSettings.GoogleSheet,
            _applicationSettings.GoogleSheetQuestTable);
        
        BootstrapActions.OnShowInfo?.Invoke("Loading Answers");
        configDataContainer.ApplicationData.Answers = await ReadGoogleSheets.FillDataAsync<AnswersData>(
            _applicationSettings.GoogleSheet,
            _applicationSettings.GoogleSheetAnswersTable);
        
        BootstrapActions.OnShowInfo?.Invoke("Loading Resources");
        configDataContainer.ApplicationData.Resources = await ReadGoogleSheets.FillDataAsync<ResourcesData>(
            _applicationSettings.GoogleSheet,
            _applicationSettings.GoogleSheetResourcesTable);

        BootstrapActions.OnShowInfo?.Invoke("LOADED");
        BootstrapActions.OnShowInfo?.Invoke(string.Empty);
        await UniTask.Delay(2000);
    }
}