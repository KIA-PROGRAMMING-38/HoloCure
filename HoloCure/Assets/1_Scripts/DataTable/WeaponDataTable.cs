using StringLiterals;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WeaponDataTable
{
    private Dictionary<WeaponID, WeaponData> _weaponDataContainer = new Dictionary<WeaponID, WeaponData>();
    private Dictionary<WeaponID, WeaponStat> _weaponStatContainer = new Dictionary<WeaponID, WeaponStat>();
    private Dictionary<WeaponID, Weapon> _weaponPrefabContainer = new Dictionary<WeaponID, Weapon>();

    public Dictionary<WeaponID, WeaponData> WeaponDataContainer => _weaponDataContainer;
    public Dictionary<WeaponID, WeaponStat> WeaponStatContainer => _weaponStatContainer;
    public Dictionary<WeaponID, Weapon> WeaponPrefabContainer => _weaponPrefabContainer;

    public WeaponDataTable()
    {
        SetWeaponData();
    }
    private void SetWeaponData()
    {
        TextAsset csvFile = Resources.Load<TextAsset>(Path.Combine(PathLiteral.DATA_TABLE, FileNameLiteral.WEAPON));
        string[] rows = csvFile.text.Split('\n');

        for (int i = 1; i < rows.Length; ++i)
        {
            string[] columns = rows[i].Split(',');
            WeaponData data = new WeaponData();
            WeaponStat stat = new WeaponStat();

            data.ID = (WeaponID)int.Parse(columns[0]);
            data.Name = columns[1];
            data.DisplayName = columns[2];
            data.IconName = columns[3];
            data.Display = Resources.Load<Sprite>(Path.Combine(PathLiteral.SPRITE, PathLiteral.WEAPON, data.DisplayName));
            data.Icon = Resources.Load<Sprite>(Path.Combine(PathLiteral.SPRITE, PathLiteral.WEAPON, data.IconName));
            data.ProjectileClip = Resources.Load<AnimationClip>(Path.Combine(PathLiteral.ANIM, PathLiteral.WEAPON, data.Name + FileNameLiteral._PROJECTILE));
            data.EffectClip = Resources.Load<AnimationClip>(Path.Combine(PathLiteral.ANIM, PathLiteral.WEAPON, data.Name + FileNameLiteral._EFFECT));
            _weaponDataContainer.Add(data.ID, data);

            stat.BaseAttackSequenceTime = float.Parse(columns[4]);
            stat.MinAttackSequenceTime = float.Parse(columns[5]);
            stat.ProjectileCount = int.Parse(columns[6]);
            stat.DamageRate = float.Parse(columns[7]);
            stat.HitLimit = int.Parse(columns[8]);
            stat.HitCooltime = float.Parse(columns[9]);
            stat.Size = float.Parse(columns[10]);
            stat.AttackDurationTime = float.Parse(columns[11]);
            stat.ProjectileSpeed = int.Parse(columns[12]);
            stat.KnockbackDurationTime = float.Parse(columns[13]);
            stat.KnockbackSpeed = float.Parse(columns[14]);
            _weaponStatContainer.Add(data.ID, stat);

            Weapon prefab = Resources.Load<Weapon>(Path.Combine(PathLiteral.PREFAB, data.Name));
            _weaponPrefabContainer.Add(data.ID, prefab);
        }
    }
}