using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DataSakura.Runtime.Utilities;
using UnityEngine;

public class GoogleSheetLoadUnit<T> : ILoadUnit where T : class, new()
{
    private readonly string _googleSheetId;
    private string _sheetId;
    public object Data;

    public GoogleSheetLoadUnit(string googleSheetId,
        string sheetId)
    {
        _googleSheetId = googleSheetId;
        _sheetId = sheetId;
    }

    public UniTask Load()
    {
        ReadGoogleSheets.FillData<T>(_googleSheetId, _sheetId, list =>
                {
                    List<T> data = new List<T>();
                    data = list;
                    Data = data;
                }
            );
        return UniTask.CompletedTask;
    }
}