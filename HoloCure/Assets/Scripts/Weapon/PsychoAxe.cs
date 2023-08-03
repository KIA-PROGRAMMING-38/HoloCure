using UnityEngine;

public class PsychoAxe : Weapon
{
    protected override void PerformStrike(int strikeIndex)
    {
        WeaponStrike strike = Managers.Spawn.Strike.Get();
        Vector2 strikeInitPosition = weapon2DPosition;

        strike.Init(strikeInitPosition, weaponData, weaponCollider,
            StrikeOperate);

        Managers.Sound.Play(SoundID.PsychoAxe);
    }

    private void StrikeOperate(WeaponStrike strike)
    {
        strike.Angle += weaponData.StrikeSpeed * Time.deltaTime;
        strike.Radius += weaponData.Radius * Time.deltaTime;
        Vector2 direction = Utils.GetClockwiseVector(strike.Angle);
        Vector2 nextSpiralOffset = direction * strike.Radius;

        strike.transform.position = strike.InitPosition + nextSpiralOffset;
    }
}