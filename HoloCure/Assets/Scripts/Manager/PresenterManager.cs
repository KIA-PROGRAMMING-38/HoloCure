using UI.Presenter;
using UnityEngine;

public class PresenterManager : MonoBehaviour
{
    public TitleUIPresenter TitleUIPresenter { get; private set; }
    public TriggerUIPresenter TriggerUIPresenter { get; private set; }
    public ExpPresenter ExpPresenter {get; private set;}
    public CountPresenter CountPresenter { get; private set;}
    public HPPresenter HPPresenter { get; private set;}
    public TimePresenter TimePresenter { get; private set;}
    public InitPresenter InitPresenter { get; private set;}
    public StatPresenter StatPresenter { get; private set;}
    public InventoryPresenter InventoryPresenter { get; private set;}

    private void Awake()
    {
        UIBaseLegacy.PresenterManager = this;

        TitleUIPresenter = new();
        TriggerUIPresenter = new();
        ExpPresenter = new();
        CountPresenter = new();
        HPPresenter = new();
        TimePresenter = new();
        InitPresenter = new();
        StatPresenter = new();
        InventoryPresenter = new();
    }
}