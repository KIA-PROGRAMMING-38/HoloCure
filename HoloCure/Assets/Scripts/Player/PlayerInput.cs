using StringLiterals;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

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
        _moveVec.Set(Input.GetAxisRaw(InputLiteral.HORIZONTAL), Input.GetAxisRaw(InputLiteral.VERTICAL));
        MoveVec.Value = _moveVec;
    }
}