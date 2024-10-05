using UnityEngine;

public class Gyro : MonoBehaviour
{
    [SerializeField]
    private float shiftModofier = 1f;

    private Gyroscope gyro;

    // Start is called before the first frame update
    void Start()
    {
        gyro = Input.gyro;
        gyro.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
            //((float)System.Math.Round(gyro.rotationRateUnbiased.y, 1)).ToString();
        transform.Translate(gyro.userAcceleration.x * shiftModofier, 0f, 0f, Space.Self);
    }
}
