using UnityEngine;

public class ArrowRotation : MonoBehaviour
{
    void Update()
    {
        float angle = Util.Caching.GetAngleToMouse(transform.position);

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
