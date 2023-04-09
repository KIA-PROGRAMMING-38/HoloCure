using StringLiterals;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyDataTable
{
    private Dictionary<EnemyID, EnemyData> _enemyDataContainer = new Dictionary<EnemyID, EnemyData>();
    private Dictionary<EnemyID, CharacterStat> _enemyStatContainer = new Dictionary<EnemyID, CharacterStat>();
    private Dictionary<EnemyID, EnemyFeature> _enemyFeatureContainer = new Dictionary<EnemyID, EnemyFeature>();

    private Dictionary<EnemyID, Enemy> _enemyPrefabContainer = new Dictionary<EnemyID, Enemy>();

    private Dictionary<EnemyID, EnemyRender> _enemyRenderContainer = new Dictionary<EnemyID, EnemyRender>();

    public Dictionary<EnemyID, EnemyData> EnemyDataContainer => _enemyDataContainer;
    public Dictionary<EnemyID, CharacterStat> EnemyStatContainer => _enemyStatContainer;
    public Dictionary<EnemyID, EnemyFeature> EnemyFeatureContainer => _enemyFeatureContainer;
    public Dictionary<EnemyID, Enemy> EnemyPrefabContainer => _enemyPrefabContainer;
    public Dictionary<EnemyID, EnemyRender> EnemyRenderContainer => _enemyRenderContainer;

    public EnemyDataTable()
    {
        SetEnemyData();
    }
    private void SetEnemyData()
    {
        TextAsset csvFile = Resources.Load<TextAsset>(Path.Combine(PathLiteral.DATA_TABLE, FileNameLiteral.ENEMY));
        string[] rows = csvFile.text.Split('\n');

        Enemy defaultPrefab = Resources.Load<Enemy>(Path.Combine(PathLiteral.PREFAB, FileNameLiteral.ENEMY)).GetComponent<Enemy>();

        EnemyRender.HitMaterial = Resources.Load<Material>(Path.Combine(PathLiteral.MATERIAL, FileNameLiteral.HIT_MATERIAL));

        for (int i = 1; i < rows.Length; ++i)
        {
            string[] columns = rows[i].Split(',');
            EnemyData data = new EnemyData();
            CharacterStat stat = new CharacterStat();
            EnemyFeature feature = new EnemyFeature();
            EnemyRender render = new EnemyRender();

            data.ID = (EnemyID)int.Parse(columns[0]);
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
            render.MoveClip = Resources.Load<AnimationClip>(Path.Combine(PathLiteral.ANIM, PathLiteral.CHARACTER, PathLiteral.ENEMY, data.Name + FileNameLiteral.MOVE));

            _enemyRenderContainer.Add(data.ID, render);
            
            Enemy prefab = Object.Instantiate(defaultPrefab);

            prefab.InitializeStatus(data.ID, this);
            prefab.SetEnemyRender(render);

            _enemyPrefabContainer.Add(data.ID, prefab);

            prefab.gameObject.SetActive(false);
        }
    }
}