using UnityEngine;

public class PresenterManager : MonoBehaviour
{
    public GameManager GameManager { private get; set; }
    public ExpPresenter ExpPresenter {get; private set;}
    public CountPresenter CountPresenter { get; private set;}
    public HPPresenter HPPresenter { get; private set;}
    public TimePresenter TimePresenter { get; private set;}

    private void Awake()
    {
        UIBase.PresenterManager = this;

        ExpPresenter = new();
        CountPresenter = new();
        HPPresenter = new();
        TimePresenter = new();
    }
}