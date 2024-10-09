using UnityEngine;

public class MultiQuestStarter : MonoBehaviour
{
    [SerializeField] private bool multiQuest;
    [SerializeField] private ApplicationSettings _applicationSettings;

    private void Start()
    {
       // if(!multiQuest)
       //     GetComponent<Bootstrap>().StartApplicationFromSettings(_applicationSettings);
    }
}