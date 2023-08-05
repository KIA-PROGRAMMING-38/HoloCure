using UnityEngine;
using Util;

public class HoloBomb : Weapon
{
    private float[] _angles;
    public override void LevelUp()
    {
        base.LevelUp();
        Utils.SetAnglesFromCircle(_angles, weaponData.StrikeCount);
    }

    protected override void PerformStrike(int strikeIndex)
    {
        WeaponStrike strike = Managers.Spawn.Strike.Get();
        strike.gameObject.layer = Define.Layer.IMPACT;
        strike.OnImpact -= StrikeOnImpact;
        strike.OnImpact += StrikeOnImpact;

        Quaternion rotation = Quaternion.Euler(0, 0, _angles[strikeIndex]);
        Vector2 strikeInitPosition = weapon2DPosition;
        Vector2 direction = CursorCache.GetDirectionToCursor(strikeInitPosition);
        Vector2 offset = rotation * direction * weaponData.Radius;

        strike.Init(strikeInitPosition, weaponData, weaponCollider,
            StrikeOperate, offset: offset);

        Managers.Sound.Play(SoundID.HoloBomb);
    }

    private void StrikeOperate(WeaponStrike strike)
    {
        Vector2 start = strike.InitPosition;
        Vector2 end = start + strike.Offset;
        float t = strike.OperateTime / 0.1f;

        strike.transform.position = Vector2.Lerp(start, end, t);
        strike.transform.rotation = default;
    }

    private void StrikeOnImpact(WeaponStrike strike)
    {
        strike.HasImpacted = true;
        strike.gameObject.layer = Define.Layer.WEAPON;
        strike.transform.localScale = Vector2.one * weaponData.ImpactSize;
        strike.transform.rotation = default;

        CircleCollider2D collider = strike.gameObject.GetComponentAssert<CircleCollider2D>();
        collider.radius = weaponData.Radius;
        collider.offset = new Vector2(0, weaponData.Radius / 2);
        strike.ResetCollider();
    }
}
