using StringLiterals;
using UnityEngine;

public class GridReposition : MonoBehaviour
{
    private BoxCollider2D _collier;

    private float gridMoveSize;

    private void Awake()
    {
        _collier = gameObject.GetComponentAssert<BoxCollider2D>();
        _collier.isTrigger = true;
        gridMoveSize = _collier.size.x * 2;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (false == collision.CompareTag(TagLiteral.GRID_SENSOR))
        {
            return;
        }

        PlayerInput input = collision.transform.root.GetComponentAssert<PlayerInput>();

        float offsetX = Mathf.Abs(collision.transform.root.position.x - transform.position.x);
        float offsetY = Mathf.Abs(collision.transform.root.position.y - transform.position.y);

        if (offsetX >= offsetY)
        {
            transform.Translate(Vector2.right * (input.MoveVec.Value.x * gridMoveSize));
        }
        if (offsetX <= offsetY)
        {
            transform.Translate(Vector2.up * (input.MoveVec.Value.y * gridMoveSize));
        }
    }
}
