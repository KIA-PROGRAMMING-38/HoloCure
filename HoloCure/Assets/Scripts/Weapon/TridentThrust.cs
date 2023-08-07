using UnityEngine;

public class TridentThrust : CursorTargetingMeleeWeapon
{
    private const float ANGLE_BETWEEN_STRIKES = 15f;
    private float[] _angles;
    private Quaternion _centerStrikeRotation;

    private void Awake()
    {
        int strikeMaxCount = Managers.Data.WeaponLevelTable[ItemID.TridentThrust][7].StrikeCount + 2;
        _angles = new float[strikeMaxCount];
    }

    public override void LevelUp()
    {
        base.LevelUp();
        Utils.SetAnglesFromCenter(_angles, weaponData.StrikeCount, ANGLE_BETWEEN_STRIKES);
    }

    protected override void SetupStrikeOnPerform(WeaponStrike strike, int strikeIndex)
    {
        if(strikeIndex == 0)
        {
            strike.transform.RotateLookCursor();
            _centerStrikeRotation = strike.transform.rotation;
        }
        strike.transform.rotation = _centerStrikeRotation * Quaternion.AngleAxis(_angles[strikeIndex], Vector3.back);

        Managers.Sound.Play(SoundID.TridentThrust);
    }
}