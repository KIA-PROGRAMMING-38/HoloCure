using UnityEngine;

public class LevelUpListController : MonoBehaviour
{
    [SerializeField] private LevelUpList[] _lists;
    
    private void Awake()
    {
        
    }
    private void Start()
    {
        for (int i = 0; i < _lists.Length; ++i)
        {
            for (int j = 0; j < _lists.Length; ++j)
            {
                if (i == j)
                {
                    continue;
                }

                _lists[i].OnHoverForOtherList -= _lists[j].ActivateDefaultFrame;
                _lists[i].OnHoverForOtherList += _lists[j].ActivateDefaultFrame;
            }

            _lists[i].OnClickForController -= TriggerEventByClick;
            _lists[i].OnClickForController += TriggerEventByClick;
        }
    }
    private void TriggerEventByClick(LevelUpList list)
    {
        int index = 0;
        for (int i = 0; i < _lists.Length; ++i)
        {
            if (list != _lists[i])
            {
                continue;
            }

            index = i;
            break;
        }

        
    }

    
}
