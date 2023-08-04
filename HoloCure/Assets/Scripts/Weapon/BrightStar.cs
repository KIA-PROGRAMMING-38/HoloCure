using UnityEngine;

public class BrightStar : CursorTargetingRangedWeapon
{
    private const float ANGLE_BETWEEN_STRIKES = 30f;
    private float _strikeMoveTime;
    private void Awake()
    {
        angleBetweenStrikes = ANGLE_BETWEEN_STRIKES;
        _strikeMoveTime = Managers.Data.WeaponLevelTable[ItemID.BirdFeather][1].AttackDurationTime;
    }

    protected override void SetupStrikeOnPerform(WeaponStrike strike, int strikeIndex)
    {
        strike.transform.rotation = centerStrikeRotation * Quaternion.AngleAxis(angles[strikeIndex], Vector3.back);

        Managers.Sound.Play(SoundID.BirdFeather);
    }

    protected override void StrikeOperate(WeaponStrike strike)
    {
        if (strike.OperateTime > _strikeMoveTime) { return; }

        base.StrikeOperate(strike);
    }
}