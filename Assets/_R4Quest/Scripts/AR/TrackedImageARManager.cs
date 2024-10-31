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
    public bool readyForTracking = true;
    private ResourcesService resourcesService;
    
    private void Start()
    {
        //check CustomReferenceLibrary in LocalRepository
        
        aRTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        aRTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        ARSceneActions.OnARSession += OnARSession;
        ARSceneActions.OnReadyForTracking += OnReadyForTracking;
    }

    private void OnReadyForTracking(bool state)
    {
        Debug.Log("tracking ready " + state);
        readyForTracking = state;
    }

    private void OnARSession()
    {
        ARSession.SetActive(true);
    }

    private void OnDisable()
    {
        aRTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        ARSceneActions.OnARSession -= OnARSession;
        ARSceneActions.OnReadyForTracking -= OnReadyForTracking;
    }
    
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        if(!readyForTracking)
            return;
        
        foreach (var trackedImage in eventArgs.added)
        {
            if (!trackedImages.Contains(trackedImage))
            {
                trackedImages.Add(trackedImage);
                ARSceneActions.OnARTrackedImageAdded?.Invoke(trackedImage);
                Debug.Log("recognition " + trackedImage.referenceImage.name);
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