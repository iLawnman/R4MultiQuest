using TMPro;
using UnityEngine;

public class DebugInputCommand : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;
    void Start()
    {
        _inputField.onSubmit.AddListener(x =>
        {
            Debug.Log("manually enter " + x);
            StartQuest(x);
        });
    }

    private void StartQuest(string quest)
    {
        var controller = FindAnyObjectByType<GameflowController>();
        controller.OnARTrackedImage(quest, controller.gameObject.transform);
    }
}
