using UnityEngine;

public class MultiQuestStarter : MonoBehaviour
{
    [SerializeField] private bool multiQuest;

    private void Start()
    {
       // if(!multiQuest)
       //     GetComponent<Bootstrap>().StartApplicationFromSettings(_applicationSettings);
    }
}