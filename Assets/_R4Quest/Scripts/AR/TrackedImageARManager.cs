using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TrackedImageARManager : MonoBehaviour
{
    [SerializeField] private GameObject ARSession;
    private ARTrackedImageManager aRTrackedImageManager;
    private List<ARTrackedImage> trackedImages = new List<ARTrackedImage>();

    private void Start()
    {
        aRTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        aRTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        ARSceneActions.OnARSession += OnARSession;
    }

    private void OnARSession()
    {
        ARSession.SetActive(true);
    }

    private void OnDisable()
    {
        aRTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        ARSceneActions.OnARSession -= OnARSession;
    }
    
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {

        foreach (var trackedImage in eventArgs.added)
        {
            if (!trackedImages.Contains(trackedImage))
            {
                trackedImages.Add(trackedImage);
                ARSceneActions.OnARTrackedImageAdded?.Invoke(trackedImage);
            }
        }

        foreach (var trackedImage in eventArgs.updated) 
        { 
 
        } 
        foreach (var trackedImage in eventArgs.removed) 
        { 
            if (trackedImages.Contains(trackedImage))
            {
                trackedImages.Add(trackedImage);
            }
        }
    }
}