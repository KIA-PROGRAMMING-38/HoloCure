using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public static class Extensions
{
    public static void RotateLookCursor(this Transform transform, Vector2 startPosition)
        => transform.rotation = Quaternion.AngleAxis(Util.CursurCache.GetAngleToMouse(startPosition), Vector3.forward);
    public static void BindViewEvent(this UIBehaviour view, Action<PointerEventData> action, ViewEvent type, Component component)
        => UIBase.BindViewEvent(view, action, type, component);
    public static void BindModelEvent<T>(this ReactiveProperty<T> model, Action<T> action, Component component)
        => UIBase.BindModelEvent(model, action, component);
    public static T GetRandomElement<T>(this T[] array)
        => array[Random.Range(0, array.Length)];
    public static T GetRandomElement<T>(this T[] array, int start, int end)
        => array[Random.Range(start, end)];

    private const int ENEMY_DISTINGUISH_VALUE = 1000;
    public static EnemyType GetEnemyType(this EnemyID id)
    {
        int enemy = (int)id % ENEMY_DISTINGUISH_VALUE;
        int normal = (int)EnemyID.Normal % ENEMY_DISTINGUISH_VALUE;
        int miniBoss = (int)EnemyID.MiniBoss % ENEMY_DISTINGUISH_VALUE;
        int boss = (int)EnemyID.Boss % ENEMY_DISTINGUISH_VALUE;
        int end = (int)EnemyID.End % ENEMY_DISTINGUISH_VALUE;

        if (normal < enemy && enemy < miniBoss) { return EnemyType.Normal; }
        if (miniBoss < enemy && enemy < boss) { return EnemyType.MiniBoss; }
        if (boss < enemy && enemy < end) { return EnemyType.Boss; }
        return EnemyType.None;
    }
    public static int GetStage(this EnemyID id) => (id - EnemyID.None) / ENEMY_DISTINGUISH_VALUE;
    public static ExpType GetExpType(this int expAmount)
    {
        if (expAmount >= (int)ExpType.Max) { return ExpType.Max; }
        if (expAmount >= (int)ExpType.Four) { return ExpType.Four; }
        if (expAmount >= (int)ExpType.Three) { return ExpType.Three; }
        if (expAmount >= (int)ExpType.Two) { return ExpType.Two; }
        if (expAmount >= (int)ExpType.One) { return ExpType.One; }
        return ExpType.Zero;
    }
    public static ItemType GetItemType(this ItemID id)
    {
        if (ItemID.CommonNone < id && id < ItemID.EquipmentNone) { return ItemType.Weapon; }
        if (ItemID.EquipmentNone < id && id < ItemID.StatNone) { return ItemType.Equipment; }
        if (ItemID.StatNone < id && id < ItemID.End) { return ItemType.Stat; }
        return ItemType.None;
    }
    public static WeaponType GetWeaponType(this ItemID id)
    {
        if (ItemID.CommonNone < id && id < ItemID.StartingNone) { return WeaponType.Common; }
        if (ItemID.StartingNone < id && id < ItemID.EquipmentNone) { return WeaponType.Starting; }
        return WeaponType.None;
    }
    public static SoundType GetSoundType(this SoundID id)
    {
        if (SoundID.BGM < id && id < SoundID.Common) { return SoundType.BGM; }
        if (SoundID.Common < id && id < SoundID.Effect) { return SoundType.Common; }
        if (SoundID.Effect < id && id < SoundID.End) { return SoundType.Effect; }
        return SoundType.Max;
    }
}