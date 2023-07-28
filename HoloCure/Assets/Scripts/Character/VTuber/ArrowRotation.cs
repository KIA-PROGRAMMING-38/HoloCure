using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class ArrowRotation : MonoBehaviour
{
    private void Start()
    {
        this.UpdateAsObservable()
            .Subscribe(LookCursor);
    }

    private void LookCursor(Unit unit)
    {
        if (Time.timeScale < 1) { return; }

        float angle = Util.CursorCache.GetAngleToMouse(transform.position);

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
