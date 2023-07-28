using UnityEngine;

public class FubuzillaAnimationEvent : MonoBehaviour
{
    private GameObject _warningLine;
    private void Awake() => _warningLine = transform.GetChild(0).gameObject;

    public void ShowWarningLine() => _warningLine.SetActive(true);
    public void HideWarningLine() => _warningLine.SetActive(false);
}
