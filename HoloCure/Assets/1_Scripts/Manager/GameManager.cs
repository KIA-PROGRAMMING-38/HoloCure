using UnityEngine;

public class GameManager : MonoBehaviour
{
    public VTuberManager VTuberManager { get; private set; }
    public EnemyManager EnemyManager { get; private set; }

    private void Awake()
    {
        SetupManager();
    }
    
    private void SetupManager()
    {
        GameObject gameObject;

        gameObject = new GameObject(nameof(VTuberManager));
        gameObject.transform.parent = transform;
        VTuberManager = gameObject.AddComponent<VTuberManager>();

        gameObject = new GameObject(nameof(EnemyManager));
        gameObject.transform.parent = transform;
        EnemyManager = gameObject.AddComponent<EnemyManager>();
    }
}
