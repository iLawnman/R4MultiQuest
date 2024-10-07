using Cysharp.Threading.Tasks;
using DataSakura.Runtime.Utilities;
using Unity.VisualScripting;
using UnityEngine;

public class GoogleSheetLoadUnit<T> : ILoadUnit where T : class, new()
{
    private readonly string _googleSheetId;
    private string _sheetId;
    private bool ready;
    public object Data;

    public GoogleSheetLoadUnit(string googleSheetId,
        string sheetId)
    {
        _googleSheetId = googleSheetId;
        _sheetId = sheetId;
    }

    public async UniTask Load()
    {
        T data = new T();
        ReadGoogleSheets.FillData<T>(_googleSheetId, _sheetId, list =>
        {
            Debug.Log("list " + list[0]);
            data = list[0];
            ready = true;
        });

        await UniTask.WaitUntil(() => ready);
        Data = data;
        Data = Data as T;
    }
    public async UniTask<T> Get()
    {
        T data = new T();
        ReadGoogleSheets.FillData<T>(_googleSheetId, _sheetId, list =>
        {
            data = list[0];
            ready = true;
        });

        await UniTask.WaitUntil(() => ready);
        return data;
    }
}