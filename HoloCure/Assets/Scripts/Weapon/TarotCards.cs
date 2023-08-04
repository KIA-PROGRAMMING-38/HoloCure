using UnityEngine;

public class TarotCards : CursorTargetingRangedWeapon
{
    private const float ANGLE_BETWEEN_STRIKES = 3f;
    private float _moveForwardsTime;
    private void Awake()
    {
        angleBetweenStrikes = ANGLE_BETWEEN_STRIKES;
        _moveForwardsTime = Managers.Data.WeaponLevelTable[ItemID.TarotCards][1].AttackDurationTime / 2;
    }

    protected override void SetupStrikeOnPerform(WeaponStrike strike, int strikeIndex)
    {
        strike.transform.rotation = centerStrikeRotation * Quaternion.AngleAxis(angles[strikeIndex], Vector3.back);

        Managers.Sound.Play(SoundID.TarotCards);
    }

    protected override void StrikeOperate(WeaponStrike strike)
    {
        if (strike.OperateTime <= _moveForwardsTime)
        {
            base.StrikeOperate(strike);
        }
        else
        {
            Vector2 direction = strike.transform.right;
            Vector3 translation = direction * (weaponData.StrikeSpeed * Time.deltaTime);
            strike.transform.position -= translation;
        }
    }
}