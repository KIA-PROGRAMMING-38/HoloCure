using System.Collections;
using UnityEngine;
using Util;
using UniRx;
using StringLiterals;

public class SpawnManager : MonoBehaviour
{
    private const int WIDTH = 540;
    private const int HEIGHT = 315;
    private const int ENEMY_SPAWN_OFFSET_COUNT = 36;
    private readonly WaitForSeconds ENEMY_SPAWN_INTERVAL = DelayCache.GetWaitForSeconds(1);

    private Vector3[] _enemySpawnOffsets;

    public GameObject IngameContainer { get; private set; }
    public GameObject EnemyContainer { get; private set; }
    public GameObject DamageTextContainer { get; private set; }
    public GameObject ExpContainer { get; private set; }
    public GameObject BoxContainer { get; private set; }
    public GameObject BoxEffectContainer { get; private set; }
    public GameObject EnemyDieEffectContainer { get; private set; }

    public EnemyPool Enemy { get; private set; }
    public DamageTextPool DamageText { get; private set; }
    public ExpPool Exp { get; private set; }
    public BoxPool Box { get; private set; }
    public OpenBoxCoinPool OpenBoxCoin { get; private set; }
    public OpenBoxParticlePool OpenBoxParticle { get; private set; }
    public OpenedBoxParticlePool OpenedBoxParticle { get; private set; }
    public EnemyDieEffectPool EnemyDieEffect { get; private set; }

    public GameObject OutgameContainer { get; private set; }
    public GameObject TriangleContainer { get; private set; }
    public TrianglePool Triangle { get; private set; }
    private void Start()
    {
        _spawnOpenBoxParticleCo = SpawnOpenBoxParticleCo();
        _spawnOpenedBoxParticleCo = SpawnOpenedBoxParticleCo();
        _spawnTriangleCo = SpawnTriangleCo();
    }
    public void Init()
    {
        Managers.Game.Stage.Subscribe(OnIngameStart);
        Managers.Game.Stage.Subscribe(OnOutgameStart);
    }
    private void OnIngameStart(int stage)
    {
        if (stage == 0) { return; }

        Managers.Resource.Destroy(OutgameContainer);
        InitIngamePool();
        InitOffset();

        SpawnStageEnemies(stage);
    }
    private void InitIngamePool()
    {
        IngameContainer = new GameObject("Ingame Containers");

        EnemyContainer = new GameObject("Enemy Container");
        DamageTextContainer = new GameObject("DamageText Container");
        ExpContainer = new GameObject("Exp Container");
        BoxContainer = new GameObject("Box Container");
        BoxEffectContainer = new GameObject("BoxEffect Container");
        EnemyDieEffectContainer = new GameObject("EnemyDieEffect Container");

        EnemyContainer.transform.parent = IngameContainer.transform;
        DamageTextContainer.transform.parent = IngameContainer.transform;
        ExpContainer.transform.parent = IngameContainer.transform;
        BoxContainer.transform.parent = IngameContainer.transform;
        BoxEffectContainer.transform.parent = IngameContainer.transform;
        EnemyDieEffectContainer.transform.parent = IngameContainer.transform;

        Enemy = new EnemyPool();
        DamageText = new DamageTextPool();
        Exp = new ExpPool();
        Box = new BoxPool();
        OpenBoxCoin = new OpenBoxCoinPool();
        OpenBoxParticle = new OpenBoxParticlePool();
        OpenedBoxParticle = new OpenedBoxParticlePool();

        Enemy.Init();
        DamageText.Init();
        Exp.Init();
        Box.Init();
        OpenBoxCoin.Init();
        OpenBoxParticle.Init();
        OpenedBoxParticle.Init();
    }
    private void InitOffset()
    {
        _enemySpawnOffsets = new Vector3[ENEMY_SPAWN_OFFSET_COUNT];
        int angleDiv = 360 / ENEMY_SPAWN_OFFSET_COUNT;
        for (int i = 0; i < ENEMY_SPAWN_OFFSET_COUNT; ++i)
        {
            float angle = i * angleDiv * Mathf.Rad2Deg;

            _enemySpawnOffsets[i] = new Vector3(WIDTH * Mathf.Cos(angle), HEIGHT * Mathf.Sin(angle), 0);
        }
    }
    private void SpawnStageEnemies(int stage)
    {
        foreach (var pair in Managers.Data.Enemy)
        {
            EnemyID id = pair.Key;
            EnemyType enemyType = id.GetEnemyType(stage);

            if (enemyType == EnemyType.None) { continue; }

            StartCoroutine(SpawnEnemyCo(id, enemyType));
        }
    }
    private void OnOutgameStart(int stage)
    {
        if (stage != 0) { return; }

        StopAllCoroutines();

        Managers.Resource.Destroy(IngameContainer);
        InitOutgamePool();
    }
    private void InitOutgamePool()
    {
        OutgameContainer = new GameObject("Outgame Containers");

        TriangleContainer = new GameObject("Triangle Container");

        TriangleContainer.transform.parent = OutgameContainer.transform;

        Triangle = new TrianglePool();

        Triangle.Init();
    }
    private IEnumerator SpawnEnemyCo(EnemyID id, EnemyType type)
    {
        EnemyData data = Managers.Data.Enemy[id];

        yield return DelayCache.GetWaitForSeconds(data.SpawnStartTime);

        switch (type)
        {
            case EnemyType.Normal:
                while (Managers.StageM.CurrentStageTime < data.SpawnEndTime)
                {
                    SpawnEnemy(id);
                    yield return ENEMY_SPAWN_INTERVAL;
                }
                break;
            case EnemyType.MiniBoss:
                break;
            case EnemyType.Boss:
                break;
            default:
                Debug.Assert(false, $"Invalid EnemyID | ID: {id}");
                break;
        }
    }
    private void SpawnEnemy(EnemyID id)
    {
        Enemy enemy = Enemy.Get();
        enemy.Init(id, _enemySpawnOffsets.GetRandomElement());
    }
    public void SpawnDamageText(Vector2 position, int damage, bool isCritical)
    {
        DamageText damageText = DamageText.Get();
        damageText.Init(position, damage, isCritical);
    }
    public void SpawnExp(Vector2 position, int expAmount)
    {
        while (expAmount.GetExpType() > ExpType.Max)
        {
            Exp.Get(position, (int)ExpType.Max);

            expAmount -= (int)ExpType.Max;
        }

        Exp.Get(position, expAmount);
    }
    public void SpawnBox(Vector2 position)
    {
        Box box = Box.Get();
        box.Init(position);
    }
    public void SpawnOpenBoxCoin()
    {
        for (int i = 0; i < 100; ++i)
        {
            OpenBoxCoin.Get().Init();
        }
    }
    public void SpawnOpenBoxParticle()
    {
        StartCoroutine(_spawnOpenBoxParticleCo);
    }
    private float _elapsedBoxSpawnTime;
    private const float BOX_SPAWN_TIME = 0.75f;
    private IEnumerator _spawnOpenBoxParticleCo;
    private IEnumerator SpawnOpenBoxParticleCo()
    {
        while (true)
        {
            _elapsedBoxSpawnTime = 0;

            while (_elapsedBoxSpawnTime < BOX_SPAWN_TIME)
            {
                _elapsedBoxSpawnTime += Time.unscaledDeltaTime;

                for (int i = 0; i < 4; ++i)
                {
                    OpenBoxParticle.Get().Init();
                }
                yield return null;
            }

            StopCoroutine(_spawnOpenBoxParticleCo);

            yield return null;
        }
    }
    public void SpawnOpenedBoxParticle()
    {
        StartCoroutine(_spawnOpenedBoxParticleCo);
    }
    public void StopSpawnOpenedBoxParticle()
    {
        StopCoroutine(_spawnOpenedBoxParticleCo);
    }
    private IEnumerator _spawnOpenedBoxParticleCo;
    private IEnumerator SpawnOpenedBoxParticleCo()
    {
        while (true)
        {
            OpenedBoxParticle.Get().Init();

            yield return null;
        }
    }
    public void SpawnEnemyDieEffect(Vector2 position)
    {
        EnemyDieEffect enemyDieEffect = EnemyDieEffect.Get();
        enemyDieEffect.Init(position);
    }
    public void SpawnVTuberDieEffect(Vector2 position)
    {
        VTuberDieEffect vtuberDieEffect = Managers.Resource
            .Instantiate(FileNameLiteral.VTUBER_DIE_EFFECT, IngameContainer.transform)
            .GetComponentAssert<VTuberDieEffect>();
        vtuberDieEffect.Init(position);
    }
    public void SpawnTriangle()
    {
        StartCoroutine(_spawnTriangleCo);
    }
    public void StopSpawnTriangle()
    {
        StopCoroutine(_spawnTriangleCo);
    }
    private const float TRIANGLE_SPAWN_INTERVAL = 0.5f;
    private IEnumerator _spawnTriangleCo;
    private IEnumerator SpawnTriangleCo()
    {
        while (true)
        {
            Triangle.Get().Init();

            yield return DelayCache.GetUnscaledWaitForSeconds(TRIANGLE_SPAWN_INTERVAL);
        }
    }
}