using UnityEngine;
using Util;

public class CursorTargetingRangedWeapon : Weapon
{
    protected float angleBetweenStrikes;
    protected float[] angles = new float[Define.MAX_STRIKE_COUNT];
    protected Quaternion centerStrikeRotation;

    public override void LevelUp()
    {
        base.LevelUp();
        Utils.SetAnglesFromCenter(angles, weaponData.StrikeCount, angleBetweenStrikes);
    }

    protected override void PerformStrike(int strikeIndex)
    {
        WeaponStrike strike = Managers.Spawn.Strike.Get();
        Vector2 strikeInitPosition = weapon2DPosition;

        strike.Init(strikeInitPosition, weaponData, weaponCollider,
            StrikeOperate);

        if(strikeIndex == 0)
        {
            strike.transform.RotateLookCursor();
            centerStrikeRotation = strike.transform.rotation;
        }

        SetupStrikeOnPerform(strike, strikeIndex);
    }

    protected virtual void StrikeOperate(WeaponStrike strike)
    {
        Vector2 direction = strike.transform.right;
        Vector3 translation = direction * (weaponData.StrikeSpeed * Time.deltaTime);
        strike.transform.position += translation;

        SetupStrikeOnOperate(strike);
    }

    protected virtual void SetupStrikeOnPerform(WeaponStrike strike, int strikeIndex) { }

    protected virtual void SetupStrikeOnOperate(WeaponStrike strike) { }
}