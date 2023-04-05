using StringLiterals;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyDataTable
{
    private Dictionary<EnemyID, EnemyData> _EnemyDataContainer = new Dictionary<EnemyID, EnemyData>();
    private Dictionary<EnemyID, CharacterStat> _EnemyStatContainer = new Dictionary<EnemyID, CharacterStat>();
    private Dictionary<EnemyID, EnemyFeature> _EnemyFeatureContainer = new Dictionary<EnemyID, EnemyFeature>();

    private Dictionary<EnemyID, Enemy> _EnemyPrefabContainer = new Dictionary<EnemyID, Enemy>();

    private Dictionary<EnemyID, EnemyRender> _EnemyRenderContainer = new Dictionary<EnemyID, EnemyRender>();

    public Dictionary<EnemyID, EnemyData> EnemyDataContainer => _EnemyDataContainer;
    public Dictionary<EnemyID, CharacterStat> EnemyStatContainer => _EnemyStatContainer;
    public Dictionary<EnemyID, EnemyFeature> EnemyFeatureContainer => _EnemyFeatureContainer;
    public Dictionary<EnemyID, Enemy> EnemyPrefabContainer => _EnemyPrefabContainer;
    public Dictionary<EnemyID, EnemyRender> EnemyRenderContainer => _EnemyRenderContainer;

    public EnemyDataTable()
    {
        SetEnemyData();
    }
    private void SetEnemyData()
    {
        TextAsset csvFile = Resources.Load<TextAsset>(Path.Combine(PathLiteral.DATA_TABLE, FileNameLiteral.ENEMY));
        string[] rows = csvFile.text.Split('\n');

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

            _EnemyDataContainer.Add(data.ID, data);
            _EnemyStatContainer.Add(data.ID, stat);
            _EnemyFeatureContainer.Add(data.ID, feature);

            render.Sprite = Resources.Load<Sprite>(Path.Combine(PathLiteral.SPRITE, PathLiteral.CHARACTER, PathLiteral.ENEMY, data.SpriteName));
            render.MoveClip = Resources.Load<AnimationClip>(Path.Combine(PathLiteral.ANIM, PathLiteral.CHARACTER, PathLiteral.ENEMY, data.Name, FileNameLiteral.MOVE));

            _EnemyRenderContainer.Add(data.ID, render);

            Enemy prefab = Resources.Load<Enemy>(Path.Combine(PathLiteral.PREFAB, FileNameLiteral.ENEMY)).GetComponent<Enemy>();

            prefab.InitializePrefab(stat, feature, render);

            _EnemyPrefabContainer.Add(data.ID, prefab);
        }
    }
}