using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers Instance;
    public static DataManager Data { get; private set; }
    public static ResourceManager Resource { get; private set; }
    public static SpawnManager Spawn { get; private set; }
    public static GameManager Game { get; private set; }
    public static UIManager UI { get; private set; }
    public static PresenterManager PresenterM { get; private set; }
    public static StageManager StageM { get; private set; }
    public static ItemManager ItemM { get; private set; }

    private void Awake()
    {
        Init();

        Time.timeScale = 0;

        UI.OpenPopup<TitlePopup>();
    }
    private void Init()
    {
        Instance = this;

        DontDestroyOnLoad(this);

        Data = new DataManager();
        Resource = new ResourceManager();

        Data.Init();
        Resource.Init();

        GameObject go;

        go = new GameObject(nameof(PresenterManager));
        go.transform.parent = transform;
        PresenterM = go.AddComponent<PresenterManager>();

        go = new GameObject(nameof(GameManager));
        go.transform.parent = transform;
        Game = go.AddComponent<GameManager>();
        Game.Init();

        UI = new UIManager();
        UI.Init();

        go = new GameObject(nameof(SpawnManager));
        go.transform.parent = transform;
        Spawn = go.AddComponent<SpawnManager>();
        Spawn.Init();

        go = new GameObject(nameof(StageManager));
        go.transform.parent = transform;
        StageM = go.AddComponent<StageManager>();

        go = new GameObject(nameof(ItemManager));
        go.transform.parent = transform;
        ItemM = go.AddComponent<ItemManager>();
    }
}
