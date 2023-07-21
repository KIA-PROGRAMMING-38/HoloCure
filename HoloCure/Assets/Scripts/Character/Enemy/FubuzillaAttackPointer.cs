using System;
using UnityEngine;

public class FubuzillaAttackPointer : MonoBehaviour
{
    public event Action OnShoot;
    public event Action OnReady;

    private GameObject _dangerLine;
    private void Awake() => _dangerLine = transform.GetChild(0).gameObject;
    /// <summary>
    /// ������ ���� ��輱�� Ȱ��ȭ��ŵ�ϴ�. �ִϸ��̼� �̺�Ʈ���� ȣ��˴ϴ�.
    /// </summary>
    public void ActivateDangerLine() => _dangerLine.SetActive(true);
    /// <summary>
    /// ������ ���� ��輱�� ��Ȱ��ȭ��ŵ�ϴ�. �ִϸ��̼� �̺�Ʈ���� ȣ��˴ϴ�.
    /// </summary>
    public void DeActivateDangerLine() => _dangerLine.SetActive(false);
    /// <summary>
    /// �߻�� �̺�Ʈ�� �߻���Ű�� ���� �Լ��Դϴ�. �ִϸ��̼� �̺�Ʈ���� ȣ��˴ϴ�.
    /// </summary>
    public void Shoot() => OnShoot?.Invoke();
    /// <summary>
    /// �غ�� �̺�Ʈ�� �߻���Ű�� ���� �Լ��Դϴ�. �ִϸ��̼� �̺�Ʈ���� ȣ��˴ϴ�.
    /// </summary>
    public void Ready() => OnReady?.Invoke();
}
