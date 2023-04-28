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
    private BoxPool _boxPool;

    private void Start()
    {
        _expPool = new ExpPool();
        _expPool.Initialize();
        _boxPool = new BoxPool();
        _boxPool.Initialize();
    }

    public void SpawnEXP(Vector2 pos, int expAmount)
    {
        while (expAmount > 200)
        {
            _expPool.GetExpFromPool(pos, 200);
            expAmount -= 200;
        }
        _expPool.GetExpFromPool(pos, expAmount);
    }
    public void SpawnBox(Vector2 pos)
    {
        _boxPool.GetBoxFromPool().transform.position = pos;
    }
}