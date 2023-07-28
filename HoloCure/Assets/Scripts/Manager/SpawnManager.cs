using System.Collections;
using UnityEngine;
using Util;
using UniRx;
using StringLiterals;
using Util.Pool;

public class SpawnManager : MonoBehaviour
{
    public GameObject ObjectContainer { get; private set; }
    public GameObject BoxEffectContainer { get; private set; }
    public GameObject TriangleContainer { get; private set; }

    public Pool<Enemy> Enemy { get; private set; }
    public Pool<DamageText> DamageText { get; private set; }
    public Pool<Box> Box { get; private set; }
    public Pool<OpenBoxCoin> OpenBoxCoin { get; private set; }
    public Pool<OpenBoxParticle> OpenBoxParticle { get; private set; }
    public Pool<OpenedBoxParticle> OpenedBoxParticle { get; private set; }
    public Pool<EnemyDieEffect> EnemyDieEffect { get; private set; }
    public Pool<Projectile> Projectile { get; private set; }
    public Pool<Triangle> Triangle { get; private set; }
    public ExpPool Exp { get; private set; }
    
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

        Managers.Resource.Destroy(ObjectContainer);

        InitIngamePool();
        InitOffset();

        Managers.Resource.Instantiate($"IngameEnvironment_{stage}", ObjectContainer.transform);
        SpawnStageEnemies(stage);
    }
    private void InitIngamePool()
    {
        ObjectContainer = new GameObject("Object Containers");
        BoxEffectContainer = new GameObject("BoxEffect Container");
        BoxEffectContainer.transform.parent = ObjectContainer.transform;

        Enemy = new Pool<Enemy>();
        DamageText = new Pool<DamageText>();
        Box = new Pool<Box>();
        OpenBoxCoin = new Pool<OpenBoxCoin>();
        OpenBoxParticle = new Pool<OpenBoxParticle>();
        OpenedBoxParticle = new Pool<OpenedBoxParticle>();
        EnemyDieEffect = new Pool<EnemyDieEffect>();
        Projectile = new Pool<Projectile>();
        Exp = new ExpPool();

        Enemy.Init(ObjectContainer);
        DamageText.Init(ObjectContainer);
        Box.Init(ObjectContainer);
        OpenBoxCoin.Init(BoxEffectContainer);
        OpenBoxParticle.Init(BoxEffectContainer);
        OpenedBoxParticle.Init(BoxEffectContainer);
        EnemyDieEffect.Init(ObjectContainer);
        Projectile.Init(ObjectContainer);
        Exp.Init(ObjectContainer);
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

    private void OnOutgameStart(int stage)
    {
        if (stage != 0) { return; }

        StopAllCoroutines();

        Managers.Resource.Destroy(ObjectContainer);
        InitOutgamePool();
    }
    private void InitOutgamePool()
    {
        ObjectContainer = new GameObject("Object Containers");
        TriangleContainer = new GameObject("Triangle Container");

        Triangle = new Pool<Triangle>();

        Triangle.Init(TriangleContainer);
    }

    private void SpawnStageEnemies(int stage)
    {
        foreach (var pair in Managers.Data.Enemy)
        {
            EnemyID id = pair.Key;
            if (id.GetStage() != stage) { continue; }

            EnemyType enemyType = id.GetEnemyType();
            if (enemyType == EnemyType.None) { continue; }

            StartCoroutine(SpawnEnemyCo(id, enemyType));
        }
    }
    private const int WIDTH = 540;
    private const int HEIGHT = 315;
    private const int ENEMY_SPAWN_OFFSET_COUNT = 36;
    private readonly WaitForSeconds ENEMY_SPAWN_INTERVAL = DelayCache.GetWaitForSeconds(1);
    private Vector3[] _enemySpawnOffsets;
    private IEnumerator SpawnEnemyCo(EnemyID id, EnemyType type)
    {
        EnemyData data = Managers.Data.Enemy[id];

        yield return DelayCache.GetWaitForSeconds(data.SpawnStartTime);

        switch (type)
        {
            case EnemyType.Normal:
                while (Managers.Game.CurrentStageTime < data.SpawnEndTime)
                {
                    SpawnEnemy(id);
                    yield return ENEMY_SPAWN_INTERVAL;
                }
                break;
            case EnemyType.MiniBoss:
                SpawnEnemy(id);
                break;
            case EnemyType.Boss:
                SpawnBoss(id);
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
    private void SpawnBoss(EnemyID id)
    {
        EnemyData data = Managers.Data.Enemy[id];
        GameObject go = Managers.Resource.Instantiate(data.Name, ObjectContainer.transform);
        Enemy enemy = go.GetComponentAssert<Enemy>();
        enemy.Init(id, _enemySpawnOffsets.GetRandomElement());
    }
    public void SpawnDamageText(Vector2 position, int damage, bool isCritical)
    {
        DamageText damageText = DamageText.Get();
        damageText.Init(position, damage, isCritical);
    }
    public void SpawnExp(Vector2 position, int expAmount)
    {
        while (expAmount.GetExpType() == ExpType.Max)
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

        BoxEffectContainer.SetActive(false);
        BoxEffectContainer.SetActive(true);
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
            .Instantiate(FileNameLiteral.VTUBER_DIE_EFFECT, ObjectContainer.transform)
            .GetComponentAssert<VTuberDieEffect>();
        vtuberDieEffect.Init(position);
    }
    public void SpawnTriangle(GameObject go)
    {
        TriangleContainer.transform.parent = go.transform;
        TriangleContainer.transform.localPosition = default;

        StartCoroutine(_spawnTriangleCo);
    }
    public void StopSpawnTriangle()
    {
        StopCoroutine(_spawnTriangleCo);

        TriangleContainer.SetActive(false);
        TriangleContainer.SetActive(true);
        TriangleContainer.transform.parent = ObjectContainer.transform;
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