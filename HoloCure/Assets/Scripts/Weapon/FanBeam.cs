using UnityEngine;

public class FanBeam : CursorTargetingRangedWeapon
{
    public override void LevelUp()
    {
        base.LevelUp();
        Utils.SetAnglesFromCircle(angles, weaponData.StrikeCount);
    }
    protected override void SetupStrikeOnPerform(WeaponStrike strike, int strikeIndex)
    {
        strike.transform.rotation = centerStrikeRotation * Quaternion.AngleAxis(angles[strikeIndex], Vector3.back);

        if (strikeIndex == 0)
        {
            Managers.Sound.Play(SoundID.FanBeam);
        }
    }

    protected override void StrikeOperate(WeaponStrike strike)
    {

    }
}