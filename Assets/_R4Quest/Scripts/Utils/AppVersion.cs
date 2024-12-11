using TMPro;
using UnityEngine;

public class AppVersion : MonoBehaviour
{
    void Start()
    {
        GetComponent<TMP_Text>().text = Application.version;
    }
}
