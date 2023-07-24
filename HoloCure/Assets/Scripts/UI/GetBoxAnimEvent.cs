using UnityEngine;

public class GetBoxAnimEvent : MonoBehaviour
{
    private GetBoxOngoingSubItem _subItem;
    private void Awake() => _subItem = transform.parent.GetComponentAssert<GetBoxOngoingSubItem>();
    public void OpenBox() => _subItem.OpenBox();
}
