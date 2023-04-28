using UnityEngine;

public class WeaponData : ItemData
{    
    public string DisplaySpriteName;
    public string Type;
    public new string[] Description = new string[8];
    public Sprite Display;
    public AnimationClip ProjectileClip;
    public AnimationClip EffectClip;

    public int CurrentLevel;
}