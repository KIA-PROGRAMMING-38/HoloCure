using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance;
    public static DataManager Data { get; private set; }
    public static ResourceManager Resource { get; private set; }
    public static GameManager Game { get; private set; }
    public static UIManager UI { get; private set; }
    public static SpawnManager Spawn { get; private set; }
    public static ItemManager Item { get; private set; }
    public static SoundManager Sound { get; private set; }
    private void Awake()
    {
        Init();

        Time.timeScale = 0;

        Resource.Instantiate("MouseCursor");
        UI.OpenPopup<TitlePopup>();
        Sound.Play(SoundID.TitleBGM);
    }

    private void Init()
    {
        s_instance = this;

        DontDestroyOnLoad(this);

        Data = new DataManager();
        Resource = new ResourceManager();
        UI = new UIManager();
        Item = new ItemManager();        

        GameObject go;

        go = new GameObject(nameof(GameManager));
        go.transform.parent = transform;
        Game = go.AddComponent<GameManager>();

        go = new GameObject(nameof(SpawnManager));
        go.transform.parent = transform;
        Spawn = go.AddComponent<SpawnManager>();

        go = new GameObject(nameof(SoundManager));
        go.transform.parent = transform;
        Sound = go.AddComponent<SoundManager>();

        Data.Init();
        Resource.Init();
        Game.Init();
        UI.Init();
        Spawn.Init();
        Item.Init();
        Sound.Init();
    }
}
