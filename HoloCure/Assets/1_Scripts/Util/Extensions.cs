using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public static class Extensions
{
    public static void BindViewEvent(this UIBehaviour view, Action<PointerEventData> action, ViewEvent type, Component component)
        => UIBase.BindViewEvent(view, action, type, component);
    public static void BindModelEvent<T>(this ReactiveProperty<T> model, Action<T> action, Component component)
        => UIBase.BindModelEvent(model, action, component);
    public static T GetRandomElement<T>(this T[] array)
        => array[Random.Range(0, array.Length)];
    public static T GetRandomElement<T>(this T[] array, int start, int end)
        => array[Random.Range(start, end)];
    public static EnemyType GetEnemyType(this EnemyID id, int stage)
    {
        id -= stage * 1000;

        if (EnemyID.Normal < id && id < EnemyID.MiniBoss) { return EnemyType.Normal; }
        if (EnemyID.MiniBoss < id && id < EnemyID.Boss) { return EnemyType.MiniBoss; }
        if (EnemyID.Boss < id && id < EnemyID.End) { return EnemyType.Boss; }
        return EnemyType.None;
    }
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
}