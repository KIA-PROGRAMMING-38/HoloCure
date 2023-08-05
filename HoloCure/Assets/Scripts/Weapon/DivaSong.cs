using UnityEngine;
public class DivaSong : CursorTargetingRangedWeapon
{
    private const float FREQUENCY = 2 * Mathf.PI;
    private const float AMPLITUDE = 300f;
    protected override void SetupStrikeOnPerform(WeaponStrike strike, int strikeIndex)
    {
        strike.Rotation = strike.transform.rotation;

        Managers.Sound.Play(SoundID.DivaSong);
    }
    protected override void StrikeOperate(WeaponStrike strike)
    {
        strike.transform.rotation = strike.Rotation;

        float horizontalSpeed = weaponData.StrikeSpeed * Time.deltaTime;
        float verticalSpeed = Mathf.Cos(strike.OperateTime * FREQUENCY) * AMPLITUDE * Time.deltaTime;

        Vector2 horizontalTranslation = strike.transform.right * horizontalSpeed;
        Vector2 verticalTranslation = strike.transform.up * verticalSpeed;
        Vector3 translation = horizontalTranslation + verticalTranslation;

        strike.transform.position += translation;

        strike.transform.rotation = default;
    }
}