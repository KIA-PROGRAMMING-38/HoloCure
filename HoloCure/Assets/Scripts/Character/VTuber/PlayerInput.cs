using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Util;

public class PlayerInput : MonoBehaviour
{
    public ReactiveProperty<Vector2> MoveVec { get; private set; } = new();
    private Vector2 _moveVec;
    private void Start()
    {
        this.UpdateAsObservable()
            .Subscribe(UpdateMoveVec);
    }
    private void UpdateMoveVec(Unit unit)
    {
        _moveVec.Set(Input.GetAxisRaw(Define.Input.HORIZONTAL), Input.GetAxisRaw(Define.Input.VERTICAL));
        MoveVec.Value = _moveVec;
    }
}