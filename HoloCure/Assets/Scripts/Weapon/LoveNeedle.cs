using UnityEngine;

public class LoveNeedle : CursorTargetingRangedWeapon
{
    private const float MOVING_TIME = 0.3f;
    private const float ANGLE_BETWEEN_STRIKES = 10f;
    private void Awake() => angleBetweenStrikes = ANGLE_BETWEEN_STRIKES;

    protected override void SetupStrikeOnPerform(WeaponStrike strike, int strikeIndex)
    {
        strike.transform.rotation = centerStrikeRotation * Quaternion.AngleAxis(angles[strikeIndex], Vector3.back);

        Managers.Sound.Play(SoundID.LoveNeedle);
    }

    protected override void StrikeOperate(WeaponStrike strike)
    {
        if (strike.OperateTime <= MOVING_TIME)
        {
            base.StrikeOperate(strike);
        }
    }
}