using StringLiterals;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemWeightDataTable
{
    //private Dictionary<CommonWeaponID, WeaponData> _weaponDataContainer = new Dictionary<CommonWeaponID, WeaponData>();
    //private Dictionary<CommonWeaponID, WeaponStat> _weaponStatContainer = new Dictionary<CommonWeaponID, WeaponStat>();
    //private Dictionary<CommonWeaponID, Weapon> _weaponPrefabContainer = new Dictionary<CommonWeaponID, Weapon>();

    //public Dictionary<CommonWeaponID, WeaponData> WeaponDataContainer => _weaponDataContainer;
    //public Dictionary<CommonWeaponID, WeaponStat> WeaponStatContainer => _weaponStatContainer;
    //public Dictionary<CommonWeaponID, Weapon> WeaponPrefabContainer => _weaponPrefabContainer;

    //public ItemWeightDataTable()
    //{
    //    SetWeaponData();
    //}
    //private void SetWeaponData()
    //{
    //    TextAsset csvFile = Resources.Load<TextAsset>(Path.Combine(PathLiteral.DATA_TABLE, FileNameLiteral.WEAPON));
    //    string[] rows = csvFile.text.Split('\n');

    //    for (int i = 1; i < rows.Length; i += 7)
    //    {
    //        string[] columns = rows[i].Split(',');
    //        WeaponData data = new WeaponData();
    //        WeaponStat stat = new WeaponStat();

    //        data.ID = int.Parse(columns[0]);
    //        data.Name = columns[1];
    //        data.DisplayName = columns[2];
    //        data.IconSpriteName = columns[3];
    //        data.Type = columns[4];
    //        data.Weight = int.Parse(columns[5]);
    //        data.Display = Resources.Load<Sprite>(Path.Combine(PathLiteral.SPRITE, PathLiteral.WEAPON, data.DisplayName));
    //        data.Icon = Resources.Load<Sprite>(Path.Combine(PathLiteral.SPRITE, PathLiteral.WEAPON, data.IconSpriteName));
    //        data.ProjectileClip = Resources.Load<AnimationClip>(Path.Combine(PathLiteral.ANIM, PathLiteral.WEAPON, data.Name + FileNameLiteral._PROJECTILE));
    //        data.EffectClip = Resources.Load<AnimationClip>(Path.Combine(PathLiteral.ANIM, PathLiteral.WEAPON, data.Name + FileNameLiteral._EFFECT));

    //        for (int j = i; j < i + 7; ++j)
    //        {
    //            columns = rows[j].Split(',');

    //            data.Description[j] = columns[6];
    //            stat.BaseAttackSequenceTime[j] = float.Parse(columns[7]);
    //            stat.MinAttackSequenceTime[j] = float.Parse(columns[8]);
    //            stat.ProjectileCount[j] = int.Parse(columns[9]);
    //            stat.DamageRate[j] = float.Parse(columns[10]);
    //            stat.AttackDelay[j] = float.Parse(columns[11]);
    //            stat.HitCooltime[j] = float.Parse(columns[12]);
    //            stat.Size[j] = float.Parse(columns[13]);
    //            stat.AttackDurationTime[j] = float.Parse(columns[14]);
    //            stat.ProjectileSpeed[j] = int.Parse(columns[15]);
    //            stat.KnockbackDurationTime[j] = float.Parse(columns[16]);
    //            stat.KnockbackSpeed[j] = float.Parse(columns[17]);
    //        }

    //        _weaponDataContainer.Add((CommonWeaponID)data.ID, data);
    //        _weaponStatContainer.Add((CommonWeaponID)data.ID, stat);
    //        Weapon prefab = Resources.Load<Weapon>(Path.Combine(PathLiteral.PREFAB, data.Name));
    //        _weaponPrefabContainer.Add((CommonWeaponID)data.ID, prefab);
    //    }
    //}
}