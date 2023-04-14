using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DataTableManager DataTableManager { get; private set; }
    public VTuberManager VTuberManager { get; private set; }
    public EnemyManager EnemyManager { get; private set; }
    public ObjectManager ObjectManager { get; private set; }

    private void Awake()
    {
        SetupManager();
        
    }
    private void SetupManager()
    {
        GameObject gameObject;

        gameObject = new GameObject(nameof(DataTableManager));
        gameObject.transform.parent = transform;
        DataTableManager = gameObject.AddComponent<DataTableManager>();

        gameObject = new GameObject(nameof(VTuberManager));
        gameObject.transform.parent = transform;
        VTuberManager = gameObject.AddComponent<VTuberManager>();

        gameObject = new GameObject(nameof(EnemyManager));
        gameObject.transform.parent = transform;
        EnemyManager = gameObject.AddComponent<EnemyManager>();

        gameObject = new GameObject(nameof(ObjectManager));
        gameObject.transform.parent = transform;
        ObjectManager = gameObject.AddComponent<ObjectManager>();

        VTuberManager.GameManager = this;
        EnemyManager.GameManager = this;
        ObjectManager.GameManager = this;
    }
}
