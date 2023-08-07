using UnityEngine;

public class CursorTargetingMeleeWeapon : Weapon
{
    private Vector2 _cursorDirectedOffset;
    protected override void PerformStrike(int strikeIndex)
    {
        WeaponStrike strike = Managers.Spawn.Strike.Get();

        _cursorDirectedOffset = weapon2DPosition.GetPositionToCursor(transform.localPosition.x);
        Vector2 strikeInitPosition = weapon2DPosition + _cursorDirectedOffset;

        strike.Init(strikeInitPosition, weaponData, weaponCollider,
            StrikeOperate);

        strike.transform.RotateLookCursor();

        SetupStrikeOnPerform(strike, strikeIndex);
    }

    protected virtual void StrikeOperate(WeaponStrike strike)
    {
        strike.transform.position = weapon2DPosition + _cursorDirectedOffset;

        SetupStrikeOnOperate(strike);
    }

    protected virtual void SetupStrikeOnPerform(WeaponStrike strike, int strikeIndex) { }

    protected virtual void SetupStrikeOnOperate(WeaponStrike strike) { }
}