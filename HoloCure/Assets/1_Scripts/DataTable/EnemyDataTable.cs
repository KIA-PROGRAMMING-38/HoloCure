using StringLiterals;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyDataTable
{
    #region 스테이지 1
    private List<int> _stageOneEnemyList = new();
    private List<int> _stageOneMiniBossList = new();
    private List<int> _stageOneBossList = new();

    public List<int> StageOneEnemyList => _stageOneEnemyList;
    public List<int> StageOneMiniBossList => _stageOneMiniBossList;
    public List<int> StageOneBossList => _stageOneBossList;
    #endregion

    #region 컨테이너
    private Dictionary<int, EnemyData> _enemyDataContainer = new();
    private Dictionary<int, CharacterStat> _enemyStatContainer = new();
    private Dictionary<int, EnemyFeature> _enemyFeatureContainer = new();

    private Dictionary<int, Enemy> _enemyPrefabContainer = new();

    private Dictionary<int, EnemyRender> _enemyRenderContainer = new();

    public Dictionary<int, EnemyData> EnemyDataContainer => _enemyDataContainer;
    public Dictionary<int, CharacterStat> EnemyStatContainer => _enemyStatContainer;
    public Dictionary<int, EnemyFeature> EnemyFeatureContainer => _enemyFeatureContainer;
    public Dictionary<int, Enemy> EnemyPrefabContainer => _enemyPrefabContainer;
    public Dictionary<int, EnemyRender> EnemyRenderContainer => _enemyRenderContainer;
    #endregion
    public void SetDataTable()
    {
        SetEnemyData();
        SetMiniBossData();
        SetBossData();
    }
    private void SetEnemyData()
    {
        TextAsset csvFile = Resources.Load<TextAsset>(Path.Combine(PathLiteral.DATA_TABLE, FileNameLiteral.ENEMY));
        string[] rows = csvFile.text.Split('\n');

        Enemy defaultPrefab = Resources.Load<Enemy>(Path.Combine(PathLiteral.PREFAB, FileNameLiteral.ENEMY));

        EnemyRender.HitMaterial = Resources.Load<Material>(Path.Combine(PathLiteral.MATERIAL, FileNameLiteral.HIT_MATERIAL));

        for (int i = 1; i < rows.Length; ++i)
        {
            string[] columns = rows[i].Split(',');
            EnemyData data = new();
            CharacterStat stat = new();
            EnemyFeature feature = new();
            EnemyRender render = new();

            data.ID = int.Parse(columns[0]);
            data.Name = columns[1];
            data.SpriteName = columns[2];

            stat.MaxHealth = int.Parse(columns[3]);
            stat.ATKPower = int.Parse(columns[4]);
            stat.MoveSpeedRate = float.Parse(columns[5]);
            feature.Exp = int.Parse(columns[6]);
            feature.SpawnStartTime = int.Parse(columns[7]);
            feature.SpawnEndTime = int.Parse(columns[8]);

            _enemyDataContainer.Add(data.ID, data);
            _enemyStatContainer.Add(data.ID, stat);
            _enemyFeatureContainer.Add(data.ID, feature);

            render.Sprite = Resources.Load<Sprite>(Path.Combine(PathLiteral.SPRITE, PathLiteral.CHARACTER, PathLiteral.ENEMY, data.SpriteName));
            render.MoveClip = Resources.Load<AnimationClip>(Path.Combine(PathLiteral.ANIM, PathLiteral.CHARACTER, PathLiteral.ENEMY, data.Name + FileNameLiteral._MOVE));

            _enemyRenderContainer.Add(data.ID, render);

            Enemy prefab = Object.Instantiate(defaultPrefab);

            prefab.InitializeStatus(stat, feature);
            prefab.SetEnemyRender(render);

            _enemyPrefabContainer.Add(data.ID, prefab);

            prefab.gameObject.SetActive(false);

            _stageOneEnemyList.Add(data.ID);
        }
    }
    private void SetMiniBossData()
    {
        TextAsset csvFile = Resources.Load<TextAsset>(Path.Combine(PathLiteral.DATA_TABLE, FileNameLiteral.MINI_BOSS));
        string[] rows = csvFile.text.Split('\n');

        for (int i = 1; i < rows.Length; ++i)
        {
            string[] columns = rows[i].Split(',');
            EnemyData data = new();
            CharacterStat stat = new();
            EnemyFeature feature = new();
            EnemyRender render = new();

            data.ID = int.Parse(columns[0]);
            data.Name = columns[1];
            data.SpriteName = columns[2];

            stat.MaxHealth = int.Parse(columns[3]);
            stat.ATKPower = int.Parse(columns[4]);
            stat.MoveSpeedRate = float.Parse(columns[5]);
            feature.Exp = int.Parse(columns[6]);
            feature.SpawnStartTime = int.Parse(columns[7]);

            _enemyDataContainer.Add(data.ID, data);
            _enemyStatContainer.Add(data.ID, stat);
            _enemyFeatureContainer.Add(data.ID, feature);

            render.Sprite = Resources.Load<Sprite>(Path.Combine(PathLiteral.SPRITE, PathLiteral.CHARACTER, PathLiteral.ENEMY, data.SpriteName));
            render.MoveClip = Resources.Load<AnimationClip>(Path.Combine(PathLiteral.ANIM, PathLiteral.CHARACTER, PathLiteral.ENEMY, data.Name + FileNameLiteral._MOVE));

            _enemyRenderContainer.Add(data.ID, render);

            _stageOneMiniBossList.Add(data.ID);
        }
    }
    private void SetBossData()
    {
        TextAsset csvFile = Resources.Load<TextAsset>(Path.Combine(PathLiteral.DATA_TABLE, FileNameLiteral.BOSS));
        string[] rows = csvFile.text.Split('\n');

        for (int i = 1; i < rows.Length; ++i)
        {
            string[] columns = rows[i].Split(',');
            EnemyData data = new();
            CharacterStat stat = new();
            EnemyFeature feature = new();

            data.ID = int.Parse(columns[0]);
            data.Name = columns[1];
            data.SpriteName = columns[2];

            stat.MaxHealth = int.Parse(columns[3]);
            stat.ATKPower = int.Parse(columns[4]);
            stat.MoveSpeedRate = float.Parse(columns[5]);
            feature.Exp = int.Parse(columns[6]);
            feature.SpawnStartTime = int.Parse(columns[7]);

            _enemyDataContainer.Add(data.ID, data);
            _enemyStatContainer.Add(data.ID, stat);
            _enemyFeatureContainer.Add(data.ID, feature);

            Boss prefab = Resources.Load<Boss>(Path.Combine(PathLiteral.PREFAB, data.Name));

            _enemyPrefabContainer.Add(data.ID, prefab);

            _stageOneBossList.Add(data.ID);
        }
    }
}