using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Util;

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
        if (false == collision.CompareTag(Define.Tag.ENEMY_BODY)) { return; }

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

