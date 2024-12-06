using UnityEngine;
using UnityEngine.UI;

public class DelayedGOActivator : MonoBehaviour
{
    [SerializeField] float delay;
    [SerializeField] GameObject go;
    [SerializeField] private bool startUp;

    void Start()
    {
        if (startUp)
            Invoke("activate", delay);
    }

    public void Activate()
    {
        Invoke("activate", delay);
    }

    void activate()
    {
        GetComponent<Button>().interactable = false;
        go.SetActive(true);
    }
}