using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private GameManager _gameManager;

    public GameManager GameManager
    {
        private get => _gameManager;
        set => _gameManager = value;
    }

    private ExpPool _expPool;

    private void Start()
    {
        _expPool = new ExpPool();
        _expPool.Initialize();
    }

    public void SpawnEXP(Vector2 pos, int expAmount)
    {
        _expPool.GetExpFromPool(pos,expAmount);
    }
}