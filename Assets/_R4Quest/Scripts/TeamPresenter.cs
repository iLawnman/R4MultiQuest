using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class TeamPresenter : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject teamPanel;
    [SerializeField] private InputField nameInput;
    [SerializeField] private Button txtDone;
    [SerializeField] private Button done;
    private string playerName = "0";
    private string playerTeam = "0";
    [SerializeField] private List<Button> teams;

    private void Start()
    {
        txtDone.onClick.AddListener(() => NameSubmit(nameInput.text));
        done.interactable = false;
        done.onClick.AddListener(TeamReady);
        teams.ForEach(x=> x.onClick.AddListener(() => SetTeam(x)));
    }
    
    public void LoadData()
    {
        panel.SetActive(false);
        
        if (PlayerPrefs.HasKey("PlayerName"))
            playerName = PlayerPrefs.GetString("PlayerName");
        if(PlayerPrefs.HasKey("PlayerTeam"));
            playerTeam = PlayerPrefs.GetString("PlayerTeam");
            
    }
    private void SetTeam(Button button)
    {
        playerTeam = button.name;
        PlayerPrefs.SetString("PlayerTeam", playerTeam);
        SetActiveButton(button);
        CheckReady();
    }

    private void SetActiveButton(Button button)
    {
        button.transform.parent.GetComponentsInChildren<Button>().ToList().ForEach(x =>
        {
            x.image.color = x.colors.disabledColor;
            x.GetComponentInChildren<Text>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        });
        button.image.color = button.colors.selectedColor;
        button.GetComponentInChildren<Text>().text = button.name;
        button.GetComponentInChildren<Text>().color = button.colors.selectedColor;
    }

    private void TeamReady() => panel.SetActive(false);

    void NameSubmit(string _name)
    {
        playerName = _name;
        PlayerPrefs.SetString("PlayerName", playerName);
        teamPanel.SetActive(true);
    }

    private void CheckReady()
    {
        if (playerName != "0" && playerTeam != "0")
        {
            done.interactable = true;
            done.GetComponent<Image>().color = Color.yellow;
            done.GetComponentInChildren<Text>().color = Color.yellow;
        }
    }

    public void SetPanelActive(bool b) => panel.SetActive(b);
}
