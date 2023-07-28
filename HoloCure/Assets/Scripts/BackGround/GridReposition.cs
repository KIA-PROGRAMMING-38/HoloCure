using StringLiterals;
using UniRx.Triggers;
using UniRx;
using UnityEngine;

public class GridReposition : MonoBehaviour
{
    private BoxCollider2D _collier;

    private float _gridMoveSize;

    private void Awake()
    {
        _collier = gameObject.GetComponentAssert<BoxCollider2D>();
        _collier.isTrigger = true;

        _gridMoveSize = _collier.size.x * 2;
    }

    private void Start()
    {
        this.OnTriggerExit2DAsObservable()
            .Subscribe(OnExitTrigger);
    }

    private void OnExitTrigger(Collider2D collision)
    {
        if (false == collision.CompareTag(TagLiteral.GRID_SENSOR)) { return; }

        Transform playerTransform = collision.transform.root;
        PlayerInput input = playerTransform.GetComponentAssert<PlayerInput>();

        float offsetX = Mathf.Abs(playerTransform.position.x - transform.position.x);
        float offsetY = Mathf.Abs(playerTransform.position.y - transform.position.y);

        if (offsetX >= offsetY)
        {
            transform.Translate(Vector2.right * (input.MoveVec.Value.x * _gridMoveSize));
        }
        if (offsetX <= offsetY)
        {
            transform.Translate(Vector2.up * (input.MoveVec.Value.y * _gridMoveSize));
        }
    }
}
