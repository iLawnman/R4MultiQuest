using System.Collections;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class ScreenShotShare : MonoBehaviour
{
    [SerializeField] private Button shareButton;
    [SerializeField] string Subject, ShareMessage;
    [SerializeField] string ScreenshotName = "R4QuestScreenshot.png";
    [SerializeField] private AudioClip _camShot;

    private void Awake()
    {
        shareButton.onClick.AddListener(Share);
    }

    public void Share()
    {
        StartCoroutine(TakeSSAndShare());
        GetComponent<AudioSource>().PlayOneShot(_camShot);
    }
    
    private IEnumerator TakeSSAndShare()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

        yield return new WaitForEndOfFrame();
        
        ss.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, System.DateTime.Today.ToLongTimeString().ToString() + "_" +ScreenshotName);
        File.WriteAllBytes(filePath, ss.EncodeToPNG());
	
        // To avoid memory leaks
        Destroy(ss);

        new NativeShare().AddFile(filePath).SetSubject(Subject).SetText(ShareMessage).Share();
    }
}