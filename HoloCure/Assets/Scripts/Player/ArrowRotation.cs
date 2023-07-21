using System.Collections;
using UnityEngine;

public class ArrowRotation : MonoBehaviour
{
    private void OnEnable()
    {
        _lookAtMouseCursorCoroutine = LookAtMouseCursorCoroutine();
        StartCoroutine(_lookAtMouseCursorCoroutine);
    }
    private void OnDisable()
    {
        StopCoroutine(_lookAtMouseCursorCoroutine);
    }
    private IEnumerator _lookAtMouseCursorCoroutine;
    private IEnumerator LookAtMouseCursorCoroutine()
    {
        while (true)
        {
            float angle = Util.Caching.GetAngleToMouse(transform.position);

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            yield return Util.DelayCache.GetWaitForSeconds(0.01f);
        }
    }
}
