using UnityEngine;
using Util;

public class FoxTail : CursorTargetingMeleeWeapon
{
    private float[] _angles;
    private Quaternion _firstStrikeRotation;

    private void Awake()
    {
        int strikeMaxCount = Managers.Data.WeaponLevelTable[ItemID.TridentThrust][Define.ITEM_MAX_LEVEL].StrikeCount + 2;
        _angles = new float[strikeMaxCount];
    }

    public override void LevelUp()
    {
        base.LevelUp();
        Utils.SetAnglesFromCircle(_angles, weaponData.StrikeCount);
    }

    protected override void SetupStrikeOnPerform(WeaponStrike strike, int strikeIndex)
    {
        if (strikeIndex == 0)
        {
            _firstStrikeRotation = strike.transform.rotation;
        }
        strike.transform.rotation = _firstStrikeRotation * Quaternion.AngleAxis(_angles[strikeIndex], Vector3.back);

        Managers.Sound.Play(SoundID.FoxTail);
    }
}