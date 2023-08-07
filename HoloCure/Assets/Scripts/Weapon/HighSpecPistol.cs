using UnityEngine;
using Util;

public class HighSpecPistol : CursorTargetingRangedWeapon
{
    protected override void SetupStrikeOnPerform(WeaponStrike strike, int strikeIndex)
    {
        strike.transform.rotation = centerStrikeRotation;

        if (Level.Value == Define.ITEM_MAX_LEVEL)
        {
            Vector3 cursorDirectedOffset = weapon2DPosition.GetPositionToCursor(transform.localPosition.x);
            strike.transform.position += cursorDirectedOffset;
            Managers.Sound.Play(SoundID.HighSpecPistol);
        }
        else
        {
            Managers.Sound.Play(SoundID.PistolShot);
        }
    }
}