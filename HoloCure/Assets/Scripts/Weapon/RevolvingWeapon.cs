using UnityEngine;

public class RevolvingWeapon : Weapon
{
    protected float[] angles = new float[MAX_STRIKE_COUNT];
    protected Vector2[] strikeRevolvingOffsets = new Vector2[MAX_STRIKE_COUNT];

    public override void LevelUp()
    {
        base.LevelUp();
        Utils.SetAnglesFromCircle(angles, weaponData.StrikeCount);
        SetStrikeRevolvingOffset();
    }

    private void SetStrikeRevolvingOffset()
    {
        for (int strikeIndex = 0; strikeIndex < weaponData.StrikeCount; ++strikeIndex)
        {
            Vector2 direction = Utils.GetClockwiseVector(angles[strikeIndex]);
            strikeRevolvingOffsets[strikeIndex] = direction * weaponData.Radius;
        }
    }

    protected override void PerformStrike(int strikeIndex)
    {
        WeaponStrike strike = Managers.Spawn.Strike.Get();
        Vector2 strikeInitPosition = weapon2DPosition + strikeRevolvingOffsets[strikeIndex];

        strike.Init(strikeInitPosition, weaponData, weaponCollider,
            StrikeOperate, offset: strikeRevolvingOffsets[strikeIndex]);

        SetupStrikeOnPerform(strike, strikeIndex);
    }

    protected virtual void StrikeOperate(WeaponStrike strike)
    {
        float angle = weaponData.StrikeSpeed * Time.deltaTime;
        strike.Offset = GetNextRevolvingOffset(strike.Offset, angle);

        strike.transform.position = weapon2DPosition + strike.Offset;

        SetupStrikeOnOperate(strike);

        static Vector2 GetNextRevolvingOffset(Vector2 offset, float degrees)
        {
            float angle = degrees * Mathf.Deg2Rad;

            float sin = Mathf.Sin(angle);
            float cos = Mathf.Cos(angle);

            float newX = (cos * offset.x) + (sin * offset.y);
            float newY = (-sin * offset.x) + (cos * offset.y);

            return new Vector2(newX, newY);
        }
    }

    protected virtual void SetupStrikeOnPerform(WeaponStrike strike, int strikeIndex) { }

    protected virtual void SetupStrikeOnOperate(WeaponStrike strike) { }
}