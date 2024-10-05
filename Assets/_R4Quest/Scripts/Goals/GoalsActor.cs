using UnityEngine;

public class GoalsActor : MonoBehaviour
{
    public int goalIndex = 0;
    private GoalsController goalControl;

    private void Start()
    {
        goalControl = FindAnyObjectByType<GoalsController>(FindObjectsInactive.Include);
        goalControl.SetGoalsActive(goalIndex);
    }
}
