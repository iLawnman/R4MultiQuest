using UnityEngine;

public class GoalsActor : MonoBehaviour
{
    public int goalIndex = 0;
    private GoalsUI goalControl;

    private void Start()
    {
        goalControl = FindAnyObjectByType<GoalsUI>(FindObjectsInactive.Include);
        //goalControl.SetGoalsActive(goalIndex);
    }
}
