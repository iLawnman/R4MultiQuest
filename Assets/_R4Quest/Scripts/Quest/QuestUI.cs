using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private GameObject questStartPanel;
    void Start()
    {
        ARSceneActions.OnARTrackedImageAdded += OnARTrackedImageAdded;
    }
    void OnDestroy()
    {
        ARSceneActions.OnARTrackedImageAdded -= OnARTrackedImageAdded;
    }

    private void OnARTrackedImageAdded(ARTrackedImage obj)
    {
        Debug.Log("tracked " + obj.referenceImage.name);
    }
}
