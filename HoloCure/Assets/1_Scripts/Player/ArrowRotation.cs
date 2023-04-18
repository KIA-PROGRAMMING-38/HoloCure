using System.Collections;
using UnityEngine;

public class ArrowRotation : MonoBehaviour
{
    private void Start()
    {
        _lookAtMouseCursorCoroutine = LookAtMouseCursorCoroutine();
        StartCoroutine( _lookAtMouseCursorCoroutine );
    }
    private IEnumerator _lookAtMouseCursorCoroutine;
    private IEnumerator LookAtMouseCursorCoroutine()
    {
        while (true)
        {
            float angle = Util.Caching.GetAngleToMouse(transform.position);

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            yield return Util.TimeStore.GetWaitForSeconds(0.01f);
        }
    }
}
