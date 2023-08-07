using UnityEngine;
using Util;

public class FoxTail : CursorTargetingMeleeWeapon
{
    private float[] _angles = new float[Define.MAX_STRIKE_COUNT];
    private Quaternion _firstStrikeRotation;

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