using System;

public class BootstrapActions
{
    public static Action<ApplicationSettings> OnSelectApplication;
    public static Action<string> OnShowInfo;
    public static Action<string> AddApplicationToList;
    public static Action<string> OnSelectApplicationByName;
}