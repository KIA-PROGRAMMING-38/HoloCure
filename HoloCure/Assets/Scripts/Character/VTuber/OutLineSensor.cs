using StringLiterals;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class OutLineSensor : MonoBehaviour
{
    private const float FACTOR = 1.8f;

    private void Start()
    {
        this.OnTriggerEnter2DAsObservable()
            .Subscribe(OnTrigger);
    }

    private void OnTrigger(Collider2D collision)
    {
        if (false == collision.CompareTag(TagLiteral.ENEMY_BODY)) { return; }

        Enemy enemy = collision.transform.parent.GetComponentAssert<Enemy>();
        VTuber vtuber = Managers.Game.VTuber;

        RepositionEnemy(enemy.transform, vtuber.transform);
        enemy.Flip();
    }

    private void RepositionEnemy(Transform enemyTransform, Transform vtuberTransform)
    {
        enemyTransform.position += (vtuberTransform.position - enemyTransform.position) * FACTOR;
    }
}

