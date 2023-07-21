using UnityEngine;

public class EnemyDieEffect : MonoBehaviour
{
    private Vector3 _defaultPosition;
    private void Awake() => _defaultPosition = transform.position;
    public void Init(Vector3 position) => transform.position = _defaultPosition + position;
    public void Release() => Managers.Spawn.EnemyDieEffect.Release(this);
}