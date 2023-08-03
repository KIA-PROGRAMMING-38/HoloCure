using UnityEngine;

public class SpiderCooking : Weapon
{
    private WeaponStrike _currentStrike;

    public override void LevelUp()
    {
        base.LevelUp();
        SetSize();
    }

    private void SetSize()
    {
        transform.GetChild(0).localScale = Vector2.one * weaponData.Size;

        if (Level.Value == 1) { return; }
        _currentStrike.transform.localScale = Vector2.one * weaponData.Size;
    }

    protected override void PerformStrike(int strikeIndex)
    {
        WeaponStrike strike = Managers.Spawn.Strike.Get();
        Vector2 strikeInitPosition = weapon2DPosition;
        
        _currentStrike = strike;

        strike.Init(strikeInitPosition, weaponData, weaponCollider,
            StrikeOperate);
    }

    private float _elapsedTime;
    private void StrikeOperate(WeaponStrike strike)
    {
        strike.transform.position = weapon2DPosition;

        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > weaponData.HitCoolTime)
        {
            _elapsedTime = 0;
            strike.ResetCollider();
        }
    }
}
