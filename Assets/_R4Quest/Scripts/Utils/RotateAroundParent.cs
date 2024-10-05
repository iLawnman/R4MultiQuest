using UnityEngine;

public class RotateAroundParent : MonoBehaviour
{
    public Transform parent;
    public float x, y, z;

    // Update is called once per frame
    void Update()
    {
        if(parent)
            transform.RotateAround(parent.position, parent.transform.up, z);
        else
            transform.Rotate(new Vector3(x, y, z), Space.Self);
    }
}
