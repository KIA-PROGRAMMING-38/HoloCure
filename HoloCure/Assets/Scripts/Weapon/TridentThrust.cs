using UnityEngine;
using Util;

public class TridentThrust : CursorTargetingMeleeWeapon
{
    private const float ANGLE_BETWEEN_STRIKES = 15f;
    private float[] _angles = new float[Define.MAX_STRIKE_COUNT];
    private Quaternion _centerStrikeRotation;

    public override void LevelUp()
    {
        base.LevelUp();
        Utils.SetAnglesFromCenter(_angles, weaponData.StrikeCount, ANGLE_BETWEEN_STRIKES);
    }

    protected override void SetupStrikeOnPerform(WeaponStrike strike, int strikeIndex)
    {
        if(strikeIndex == 0)
        {
            _centerStrikeRotation = strike.transform.rotation;
        }
        strike.transform.rotation = _centerStrikeRotation * Quaternion.AngleAxis(_angles[strikeIndex], Vector3.back);

        Managers.Sound.Play(SoundID.TridentThrust);
    }
}