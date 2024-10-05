using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestSelectorUI : MonoBehaviour
{
    [SerializeField] private List<ApplicationSettings> _applicationSettings;
    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Bootstrap bootstrap;

    void Start()
    {
        bootstrap = FindObjectOfType<Bootstrap>();
        foreach (var application in _applicationSettings)
        {
            var button = Instantiate(buttonPrefab, buttons.transform);

            var tmpTxt = button.GetComponentInChildren<TMP_Text>();
            tmpTxt.text = application.applicationName;
            button.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                OnSelect(tmpTxt.text);
            });
        }
    }

    private void OnSelect(string tmpTxt)
    {
        var setting = _applicationSettings.FirstOrDefault(x => x.applicationName == tmpTxt);
        bootstrap.StartApplicationFromSettings(setting);
        gameObject.SetActive(false);
    }
}
