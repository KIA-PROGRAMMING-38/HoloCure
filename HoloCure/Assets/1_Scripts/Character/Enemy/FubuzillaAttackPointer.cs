using System;
using UnityEngine;

public class FubuzillaAttackPointer : MonoBehaviour
{
    public event Action OnShoot;
    public event Action OnReady;

    private GameObject _dangerLine;
    private void Awake() => _dangerLine = transform.GetChild(0).gameObject;
    /// <summary>
    /// 레이저 위험 경계선을 활성화시킵니다. 애니메이션 이벤트에서 호출됩니다.
    /// </summary>
    public void ActivateDangerLine() => _dangerLine.SetActive(true);
    /// <summary>
    /// 레이저 위험 경계선을 비활성화시킵니다. 애니메이션 이벤트에서 호출됩니다.
    /// </summary>
    public void DeActivateDangerLine() => _dangerLine.SetActive(false);
    /// <summary>
    /// 발사시 이벤트를 발생시키기 위한 함수입니다. 애니메이션 이벤트에서 호출됩니다.
    /// </summary>
    public void Shoot() => OnShoot?.Invoke();
    /// <summary>
    /// 준비시 이벤트를 발생시키기 위한 함수입니다. 애니메이션 이벤트에서 호출됩니다.
    /// </summary>
    public void Ready() => OnReady?.Invoke();
}
