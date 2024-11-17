using Cysharp.Threading.Tasks;
using DataSakura.Runtime.Utilities;
using UnityEngine.AddressableAssets;

public class DependencesLoadUnit : ILoadUnit
{
    private string AddressableKey;
    public DependencesLoadUnit(string applicationSettingsAddressableKey)
    {
        AddressableKey = applicationSettingsAddressableKey;
    }
        
    public async UniTask Load()
    {
        BootstrapActions.OnShowInfo?.Invoke("Addressable Init");

        await Addressables.InitializeAsync();
            
        await Addressables.DownloadDependenciesAsync(AddressableKey, true);
        
        BootstrapActions.OnShowInfo?.Invoke("Addressable Loading");
         //
         // while (!_depHandler.IsDone)
         // {
         //     BootstrapActions.OnShowInfo?.Invoke("Loading Dependencies\n" + (_depHandler.PercentComplete * 100).ToString("F0"));
         // }
    }
}