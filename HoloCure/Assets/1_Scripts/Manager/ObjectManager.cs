using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private ExpPool _expPool;
    private BoxPool _boxPool;
    private GameObject _expContainer;
    private GameObject _boxContainer;
    private void Start()
    {
        _expContainer = new GameObject("Exp Container");
        _expPool = new ExpPool();
        _expPool.Initialize(_expContainer);
        _boxContainer = new GameObject("Box Container");
        _boxPool = new BoxPool();
        _boxPool.Initialize(_boxContainer);

        Managers.PresenterM.TitleUIPresenter.OnPlayGame -= GameStart;
        Managers.PresenterM.TitleUIPresenter.OnPlayGame += GameStart;

        Managers.PresenterM.TriggerUIPresenter.OnGameEnd -= GameEnd;
        Managers.PresenterM.TriggerUIPresenter.OnGameEnd += GameEnd;
    }
    private void GameEnd()
    {
        _expContainer.SetActive(false);
        _boxContainer.SetActive(false);
    }
    private void GameStart()
    {
        _expContainer.SetActive(true);
        _boxContainer.SetActive(true);
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