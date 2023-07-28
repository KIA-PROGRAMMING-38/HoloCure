using UnityEngine;

public class GetBoxAnimationEvent : MonoBehaviour
{
    public void UpScale() => transform.localScale = new Vector3(1.3f, 1.9f, 1);
    public void OpenBox() => transform.parent.GetComponentAssert<GetBoxOngoingSubItem>().OpenBox();
}
