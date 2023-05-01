using UnityEngine;

public class VTuberDieEffect : MonoBehaviour
{
    private Transform[] _transforms;
    private void Awake()
    {
        _transforms = GetComponentsInChildren<Transform>();

        _endPoints = new Vector2[_transforms.Length];
        int angleStep = 360 / (_transforms.Length - 1);
        for (int i = 1; i < _transforms.Length; ++i)
        {
            _endPoints[i] = new(Mathf.Cos(i *  angleStep * Mathf.Deg2Rad) * 400, Mathf.Sin(i * angleStep * Mathf.Deg2Rad) * 400);
        }
    }
    private readonly Vector2 START_POINT = new(0, 0);
    private Vector2[] _endPoints;
    private void OnEnable()
    {
        for (int i = 1; i < _transforms.Length; ++i)
        {
            _transforms[i].transform.localPosition = default; 
        }
    }
    private float _elapsedTime;
    private const int DYING_TIME = 3;
    private void Update()
    {
        for (int i = 1; i < _transforms.Length; ++i)
        {
            _transforms[i].transform.localPosition = Vector2.Lerp(START_POINT, _endPoints[i], _elapsedTime / DYING_TIME);
        }

        _elapsedTime += Time.unscaledDeltaTime;
    }
}
