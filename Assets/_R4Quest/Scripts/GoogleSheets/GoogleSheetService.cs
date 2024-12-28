using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using UnityEngine;

public class GoogleSheetsService
{
    //https://script.google.com/macros/s/AKfycbw16DHsypmU4oIe25yNugWyCYhgpG3GxNBebTXdpyzYLja5tb6JuwBTHFkLwJ84SJTArA/exec
    
    private const string ApplicationName = "Unity Google Sheets Integration";
    private SheetsService sheetsService;
    private DriveService driveService;
    
    public GoogleSheetsService(string credentialsPath)
    {
        //var credential = Authenticate(credentialsPath);
        var credential = AuthenticateServiceAccount(credentialsPath);
        sheetsService = new SheetsService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName
        });
    
        driveService = new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName
        });
    }
    
    private ServiceAccountCredential AuthenticateServiceAccount(string credentialsPath)
    {
        using var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read);
        var credential = GoogleCredential.FromStream(stream)
            .CreateScoped(SheetsService.Scope.SpreadsheetsReadonly, DriveService.Scope.DriveReadonly)
            .UnderlyingCredential as ServiceAccountCredential;

        return credential;
    }
    private UserCredential Authenticate(string credentialsPath)
    {
        using var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read);
        //string credPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".credentials/sheets.googleapis.com.json");
        string credPath = Path.Combine(Application.persistentDataPath, ".credentials/sheets.googleapis.com.json");
    
        return GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.Load(stream).Secrets,
            new[] { SheetsService.Scope.SpreadsheetsReadonly, DriveService.Scope.DriveReadonly },
            "user",
            CancellationToken.None,
            new FileDataStore(credPath, true)).Result;
    }
    
    public List<string> GetImageLinks(string spreadsheetId, string sheetName)
    {
        Debug.Log("get image links " + spreadsheetId + " " + sheetName);

        //var request = sheetsService.Spreadsheets.Values.Get(spreadsheetId, sheetName);
        var request = sheetsService.Spreadsheets.Values.Get(spreadsheetId, sheetName);
        var response = request.Execute();
        Debug.Log("responce " + response.Values);

        var links = new List<string>();
        foreach (var row in response.Values)
        {
            foreach (var cell in row)
            {
                if(Uri.IsWellFormedUriString(cell.ToString(), UriKind.RelativeOrAbsolute))
                    Debug.Log("cell " + cell.GetType() + " / " + cell);

                // if (cell is string cellValue && Uri.IsWellFormedUriString(cellValue, UriKind.Absolute))
                // {
                //     Debug.Log("add " + cellValue);
                //     links.Add(cellValue);
                // }
            }
        }
    
        return links;
    }
}