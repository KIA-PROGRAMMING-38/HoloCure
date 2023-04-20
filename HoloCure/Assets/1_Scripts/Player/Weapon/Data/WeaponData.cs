using UnityEngine;

public class WeaponData
{
    public int ID;
    public string Name;
    public string DisplayName;
    public string DisplaySpriteName;
    public string IconSpriteName;
    public string Type;
    public int Weight;
    public string[] Description = new string[8];
    public Sprite Display;
    public Sprite Icon;
    public AnimationClip ProjectileClip;
    public AnimationClip EffectClip;

    public int CurrentLevel;
}