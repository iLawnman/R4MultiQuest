using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField] private int speed = 20;
    void Update()
    {
        transform.RotateAround(transform.position, transform.forward, speed * Time.deltaTime);
    }
}