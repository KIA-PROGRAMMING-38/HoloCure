using UnityEngine;

public class PoolManager
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

    public void Init()
    {
        AddEvent();
    }
    private void AddEvent()
    {
        RemoveEvent();

        Managers.Game.OnIngameStart += OnIngameStart;
        Managers.Game.OnOutgameStart += OnOutgameStart;
    }
    private void RemoveEvent()
    {
        Managers.Game.OnIngameStart -= OnIngameStart;
        Managers.Game.OnOutgameStart -= OnOutgameStart;
    }
    private void OnIngameStart()
    {
        Managers.Resource.Destroy(_outgameContainer);
        InitIngame();

        void InitIngame()
        {
            _ingameContainer = new GameObject("Ingame Containers");

            EnemyContainer = new GameObject("Enemy Container");
            DamageTextContainer = new GameObject("DamageText Container");
            ExpContainer = new GameObject("Exp Container");
            BoxContainer = new GameObject("Box Container");

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
    }
    private void OnOutgameStart()
    {
        Managers.Resource.Destroy(_ingameContainer);
        InitOutgame();

        void InitOutgame()
        {
            _outgameContainer = new GameObject("Outgame Containers");
        }
    }
}