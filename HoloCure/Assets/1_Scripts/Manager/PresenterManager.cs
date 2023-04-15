using UnityEngine;

public class PresenterManager : MonoBehaviour
{
    public GameManager GameManager { private get; set; }
    public ExpPresenter ExpPresenter {get; private set;}
    public PlayerLevelPresenter PlayerLevelPresenter { get; private set;}

    private void Awake()
    {
        UIBase.PresenterManager = this;

        ExpPresenter = new();
        PlayerLevelPresenter = new();
    }
}