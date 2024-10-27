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
    [SerializeField] private BootstrapUI bootstrap;

    void Start()
    {
        bootstrap = FindObjectOfType<BootstrapUI>(includeInactive:true);
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
        bootstrap.gameObject.SetActive(true);
        var setting = _applicationSettings.FirstOrDefault(x => x.applicationName.Contains(tmpTxt));
        BootstrapActions.OnSelectApplication?.Invoke(setting);
        gameObject.SetActive(false);
    }
}
