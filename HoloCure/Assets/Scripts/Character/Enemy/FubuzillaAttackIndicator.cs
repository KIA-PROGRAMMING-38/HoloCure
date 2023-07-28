using UnityEngine;

public class FubuzillaAttackIndicator : MonoBehaviour
{
    private GameObject _warningLine;
    private void Awake() => _warningLine = transform.GetChild(0).gameObject;
    /// <summary>
    /// 애니메이션 이벤트에서 호출됩니다.
    /// </summary>
    public void ShowWarningLine() => _warningLine.SetActive(true);
    /// <summary>
    /// 애니메이션 이벤트에서 호출됩니다.
    /// </summary>
    public void HideWarningLine() => _warningLine.SetActive(false);
}
