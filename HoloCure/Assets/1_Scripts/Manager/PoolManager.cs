using StringLiterals;
using UnityEngine;

// GameManager 생성 후 로직을 짤 수 있습니다.
public class PoolManager : MonoBehaviour
{
    private GameObject _ingameContainer;
    private GameObject _outgameContainer;
    public GameObject EnemyContainer { get; private set; }
    public GameObject DamageTextContainer { get; private set; }
    public GameObject ExpContainer { get; private set; }
    public GameObject BoxContainer { get; private set; }

    public EnemyPool Enemy { get; private set; }
    public DamageTextPool DamageText { get; private set; }
    public ExpPool Exp { get; private set; }
    public BoxPool Box { get; private set; }

    private void Start()
    {
        InitInGamePools();
        InitOutGamePools();
    }
    private void InitInGamePools()
    {
        EnemyContainer = new GameObject("Enemy Container");
        DamageTextContainer = new GameObject("DamageText Container");
        ExpContainer = new GameObject("Exp Container");
        BoxContainer = new GameObject("Box Container");


        _ingameContainer = new GameObject("Ingame Containers");
        EnemyContainer.transform.parent = _ingameContainer.transform;
        DamageTextContainer.transform.parent = _ingameContainer.transform;
        ExpContainer.transform.parent = _ingameContainer.transform;
        BoxContainer.transform.parent = _ingameContainer.transform;

        Enemy = new EnemyPool();
        DamageText = new DamageTextPool();
        Exp = new ExpPool();
        Box = new BoxPool();

        Enemy.Init();
        DamageText.Init();
        Exp.Init();
        Box.Init();
    }
    private void InitOutGamePools()
    {
        _outgameContainer = new GameObject("Outgame Containers");
    }
}