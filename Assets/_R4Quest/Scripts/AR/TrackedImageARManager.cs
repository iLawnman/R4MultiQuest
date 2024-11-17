using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class TrackedImageARManager : MonoBehaviour
{
    [SerializeField] private GameObject aRSession;
    private ARTrackedImageManager aRTrackedImageManager;
    private List<ARTrackedImage> trackedImages = new List<ARTrackedImage>();
    public bool readyForTracking = true;
    
    private void Start()
    {
        ARSceneActions.OnARSession += OnARSession;
        ARSceneActions.OnReadyForTracking += OnReadyForTracking;
        
        //check CustomReferenceLibrary in LocalRepository
        
        aRTrackedImageManager = FindFirstObjectByType<ARTrackedImageManager>(FindObjectsInactive.Include);
        aRTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        
       
    }
    private void OnDisable()
    {
        aRTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        ARSceneActions.OnARSession -= OnARSession;
        ARSceneActions.OnReadyForTracking -= OnReadyForTracking;
    }

    private void OnReadyForTracking(bool state)
    {
        Debug.Log("tracking ready " + state);
        readyForTracking = state;
    }

    private void OnARSession()
    {
        aRSession.SetActive(true);
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
                ARSceneActions.OnARTrackedImage?.Invoke(trackedImage);
            }
        }

        foreach (var trackedImage in eventArgs.updated) 
        { 
        } 
        foreach (var trackedImage in eventArgs.removed) 
        { 
            if (trackedImages.Contains(trackedImage))
            {
                trackedImages.Remove(trackedImage);
            }
        }
    }
}