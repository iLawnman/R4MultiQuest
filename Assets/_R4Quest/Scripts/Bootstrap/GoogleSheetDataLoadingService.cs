using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

public class GoogleSheetDataLoadingService
{
    public async UniTask Loading(ApplicationSettings applicationSettings, ConfigDataContainer configDataContainer)
    {
        configDataContainer.ApplicationSettings = applicationSettings;
        // if(!AddressableUtils.AddressableResourceExists(_applicationSettings.AddressableKey).Result)
        //     return;
        await Addressables.InitializeAsync();

        Debug.Log("application " + applicationSettings.applicationName);
        bool updated = await ReadGoogleSheets.CheckTableEditedAsync(applicationSettings.GoogleSheet,
            applicationSettings.GoogleSheetQuestTable);
        
        if (!updated)
        {
            Debug.Log("no need update, but temporally");
            //return;
        }

        BootstrapActions.OnShowInfo?.Invoke("Loading Dependencies");
        Debug.Log("loading dependencies");
        var handle = Addressables.DownloadDependenciesAsync(applicationSettings.AddressableKey);

        while (!handle.IsDone)
        {
            await UniTask.Yield();
        }
        
        Debug.Log("loading data");
        BootstrapActions.OnShowInfo?.Invoke("Loading Quests");
        configDataContainer.ApplicationData.Quests = await ReadGoogleSheets.FillDataAsync<QuestData>(applicationSettings.GoogleSheet,
            applicationSettings.GoogleSheetQuestTable);
        
        BootstrapActions.OnShowInfo?.Invoke("Loading Answers");
        configDataContainer.ApplicationData.Answers = await ReadGoogleSheets.FillDataAsync<AnswersData>(
            applicationSettings.GoogleSheet,
            applicationSettings.GoogleSheetAnswersTable);
        
        BootstrapActions.OnShowInfo?.Invoke("Loading Resources");
        configDataContainer.ApplicationData.Resources = await ReadGoogleSheets.FillDataAsync<ResourcesData>(
            applicationSettings.GoogleSheet,
            applicationSettings.GoogleSheetResourcesTable);

        BootstrapActions.OnShowInfo?.Invoke("ALL LOADED");
        BootstrapActions.OnShowInfo?.Invoke(string.Empty);
        await UniTask.Delay(2000);
    }
}