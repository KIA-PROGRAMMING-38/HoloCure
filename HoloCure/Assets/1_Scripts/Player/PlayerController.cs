using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private VTuber _VTuber;
    public void Initialize(VTuber VTuber) => _VTuber = VTuber;
    private void FixedUpdate() => _VTuber.Move();
}
