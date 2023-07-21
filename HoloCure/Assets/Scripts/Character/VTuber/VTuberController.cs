using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class VTuberController : MonoBehaviour
{
    private VTuber _vtuber;
    private void Awake() => _vtuber = gameObject.GetComponentAssert<VTuber>();
    private void Start()
    {
        this.FixedUpdateAsObservable()
            .Subscribe(Move);
    }

    private void Move(Unit unit) => _vtuber.Move();
}
