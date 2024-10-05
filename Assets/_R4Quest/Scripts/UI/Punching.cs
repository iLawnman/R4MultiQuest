using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Punching : MonoBehaviour
{
    public float pause = 5;
    public float scale = 0.3f;
    public float time = 2;

    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine("punching");
    }

    IEnumerator punching ()
    {
        yield return new WaitForSecondsRealtime(Random.Range(0, 10));

        while (true)
        {
            transform.DOPunchScale(new Vector3(scale, scale, scale), time, 0, 1);

            yield return new WaitForSecondsRealtime(pause);
        }
    }
}
