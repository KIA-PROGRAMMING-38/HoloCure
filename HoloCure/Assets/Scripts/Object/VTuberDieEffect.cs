using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class VTuberDieEffect : MonoBehaviour
{
    private Transform[] _transforms;
    private Vector2 _startPoint;
    private Vector2[] _endPoints;
    private const int CLOSING_TIME = 3;
    private float _elapsedTime;
    private void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => gameObject.activeSelf)
            .Subscribe(Move);
    }
    private void Move(Unit unit)
    {
        for (int i = 1; i < _transforms.Length; ++i)
        {
            _transforms[i].transform.position = Vector2.Lerp(_startPoint, _endPoints[i], _elapsedTime / CLOSING_TIME);
        }

        _elapsedTime += Time.unscaledDeltaTime;
    }
    public void Init(Vector2 position)
    {
        _transforms = GetComponentsInChildren<Transform>();

        _startPoint = position;
        _endPoints = new Vector2[_transforms.Length];

        int angleDiv = 360 / (_transforms.Length - 1);
        for (int i = 1; i < _transforms.Length; ++i)
        {
            Debug.Assert(_transforms[i] != null);

            _transforms[i].transform.position = _startPoint;

            float angle = i * angleDiv * Mathf.Deg2Rad;
            _endPoints[i] = GetEndPoint(angle);
        }
    }
    private Vector2 GetEndPoint(float angle) => _startPoint + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 400;
}
