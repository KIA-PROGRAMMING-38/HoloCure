using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DataTableManager DataTableManager { get; private set; }
    public PlayerManager PlayerManager { get; private set; }
    public EnemyManager EnemyManager { get; private set; }
    public PresenterManager PresenterManager { get; private set; }
    public ObjectManager ObjectManager { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SetupManager();
    }
    private void SetupManager()
    {
        GameObject gameObject;

        gameObject = new GameObject(nameof(DataTableManager));
        gameObject.transform.parent = transform;
        DataTableManager = gameObject.AddComponent<DataTableManager>();

        gameObject = new GameObject(nameof(PresenterManager));
        gameObject.transform.parent = transform;
        PresenterManager = gameObject.AddComponent<PresenterManager>();
        PresenterManager.GameManager = this;

        gameObject = new GameObject(nameof(PlayerManager));
        gameObject.transform.parent = transform;
        PlayerManager = gameObject.AddComponent<PlayerManager>();
        PlayerManager.GameManager = this;

        gameObject = new GameObject(nameof(EnemyManager));
        gameObject.transform.parent = transform;
        EnemyManager = gameObject.AddComponent<EnemyManager>();
        EnemyManager.GameManager = this;

        gameObject = new GameObject(nameof(ObjectManager));
        gameObject.transform.parent = transform;
        ObjectManager = gameObject.AddComponent<ObjectManager>();
        ObjectManager.GameManager = this;
    }
}
