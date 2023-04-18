using UnityEngine;

public class PresenterManager : MonoBehaviour
{
    public TriggerPresenter TriggerPresenter { get; private set; }
    public ExpPresenter ExpPresenter {get; private set;}
    public CountPresenter CountPresenter { get; private set;}
    public HPPresenter HPPresenter { get; private set;}
    public TimePresenter TimePresenter { get; private set;}
    public InitPresenter InitPresenter { get; private set;}
    public StatPresenter StatPresenter { get; private set;}

    private void Awake()
    {
        UIBase.PresenterManager = this;

        TriggerPresenter = new();
        ExpPresenter = new();
        CountPresenter = new();
        HPPresenter = new();
        TimePresenter = new();
        InitPresenter = new();
        StatPresenter = new();
    }
}