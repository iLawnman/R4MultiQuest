using UnityEngine;

public class webLinker : MonoBehaviour
{
    public string site;

    public void ToSite ()
    {
        Application.OpenURL(site);
    }
}
