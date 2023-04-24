using StringLiterals;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyDataTable
{
    #region 스테이지 1
    private List<EnemyID> _stageOneEnemyList = new();
    private List<MiniBossID> _stageOneMiniBossList = new();

    public List<EnemyID> StageOneEnemyList => _stageOneEnemyList;
    public List<MiniBossID> StageOneMiniBossList => _stageOneMiniBossList;
    #endregion

    #region 일반 적
    private Dictionary<EnemyID, EnemyData> _enemyDataContainer = new();
    private Dictionary<EnemyID, CharacterStat> _enemyStatContainer = new();
    private Dictionary<EnemyID, EnemyFeature> _enemyFeatureContainer = new();

    private Dictionary<EnemyID, Enemy> _enemyPrefabContainer = new();

    private Dictionary<EnemyID, EnemyRender> _enemyRenderContainer = new();

    public Dictionary<EnemyID, EnemyData> EnemyDataContainer => _enemyDataContainer;
    public Dictionary<EnemyID, CharacterStat> EnemyStatContainer => _enemyStatContainer;
    public Dictionary<EnemyID, EnemyFeature> EnemyFeatureContainer => _enemyFeatureContainer;
    public Dictionary<EnemyID, Enemy> EnemyPrefabContainer => _enemyPrefabContainer;
    public Dictionary<EnemyID, EnemyRender> EnemyRenderContainer => _enemyRenderContainer;
    #endregion

    #region 미니 보스
    private Dictionary<MiniBossID, EnemyData> _miniBossDataContainer = new();
    private Dictionary<MiniBossID, CharacterStat> _miniBossStatContainer = new();
    private Dictionary<MiniBossID, EnemyFeature> _miniBossFeatureContainer = new();

    private Dictionary<MiniBossID, EnemyRender> _miniBossRenderContainer = new();

    public Dictionary<MiniBossID, EnemyData> MiniBossDataContainer => _miniBossDataContainer;
    public Dictionary<MiniBossID, CharacterStat> MiniBossStatContainer => _miniBossStatContainer;
    public Dictionary<MiniBossID, EnemyFeature> MiniBossFeatureContainer => _miniBossFeatureContainer;
    public Dictionary<MiniBossID, EnemyRender> MiniBossRenderContainer => _miniBossRenderContainer;
    #endregion

    public EnemyDataTable()
    {
        SetEnemyData();
        SetMiniBossData();
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

            _enemyDataContainer.Add((EnemyID)data.ID, data);
            _enemyStatContainer.Add((EnemyID)data.ID, stat);
            _enemyFeatureContainer.Add((EnemyID)data.ID, feature);

            render.Sprite = Resources.Load<Sprite>(Path.Combine(PathLiteral.SPRITE, PathLiteral.CHARACTER, PathLiteral.ENEMY, data.SpriteName));
            render.MoveClip = Resources.Load<AnimationClip>(Path.Combine(PathLiteral.ANIM, PathLiteral.CHARACTER, PathLiteral.ENEMY, data.Name + FileNameLiteral._MOVE));

            _enemyRenderContainer.Add((EnemyID)data.ID, render);

            Enemy prefab = Object.Instantiate(defaultPrefab);

            prefab.InitializeStatus(stat, feature);
            prefab.SetEnemyRender(render);

            _enemyPrefabContainer.Add((EnemyID)data.ID, prefab);

            prefab.gameObject.SetActive(false);

            _stageOneEnemyList.Add((EnemyID)data.ID);
        }
    }
    private void SetMiniBossData()
    {
        TextAsset csvFile = Resources.Load<TextAsset>(Path.Combine(PathLiteral.DATA_TABLE, FileNameLiteral.MINI_BOSS));
        string[] rows = csvFile.text.Split('\n');

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

            _miniBossDataContainer.Add((MiniBossID)data.ID, data);
            _miniBossStatContainer.Add((MiniBossID)data.ID, stat);
            _miniBossFeatureContainer.Add((MiniBossID)data.ID, feature);

            render.Sprite = Resources.Load<Sprite>(Path.Combine(PathLiteral.SPRITE, PathLiteral.CHARACTER, PathLiteral.ENEMY, data.SpriteName));
            render.MoveClip = Resources.Load<AnimationClip>(Path.Combine(PathLiteral.ANIM, PathLiteral.CHARACTER, PathLiteral.ENEMY, data.Name + FileNameLiteral._MOVE));

            _miniBossRenderContainer.Add((MiniBossID)data.ID, render);

            _stageOneMiniBossList.Add((MiniBossID)data.ID);
        }
    }
}