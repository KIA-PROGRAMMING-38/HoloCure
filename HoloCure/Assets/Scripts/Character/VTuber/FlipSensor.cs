using StringLiterals;
using UniRx.Triggers;
using UniRx;
using UnityEngine;

public class FlipSensor : MonoBehaviour
{
    private void Start()
    {
        this.OnTriggerEnter2DAsObservable()
            .Subscribe(OnTrigger);

        this.OnTriggerExit2DAsObservable()
            .Subscribe(OnTrigger);
    }

    private void OnTrigger(Collider2D collision)
    {
        if (false == collision.CompareTag(TagLiteral.ENEMY_BODY)) { return; }

        Enemy enemy = collision.transform.parent.GetComponentAssert<Enemy>();
        enemy.Flip();
    }
}
