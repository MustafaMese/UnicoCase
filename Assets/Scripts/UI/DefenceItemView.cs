using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class DefenceItemView : MonoBehaviour
{
    [SerializeField] private List<DefenceButton> defenceButtons;

    private CanvasGroup _canvasGroup;
    
    public async Task Initialize(List<DefenceItemType> levelDefenceItems)
    {
        var defenceItem1Count = levelDefenceItems.FindAll(x => x == DefenceItemType.DefenceItem1).Count;
        var defenceItem2Count = levelDefenceItems.FindAll(x => x == DefenceItemType.DefenceItem2).Count;
        var defenceItem3Count = levelDefenceItems.FindAll(x => x == DefenceItemType.DefenceItem3).Count;

        transform.SetParent(null);
        
        _canvasGroup = GetComponent<CanvasGroup>();

        await _canvasGroup.DOFade(1f, 0.2f).AsyncWaitForCompletion();
        
        await Task.WhenAll(
            defenceButtons[0].Initialize(DefenceItemType.DefenceItem1, defenceItem1Count),
            defenceButtons[1].Initialize(DefenceItemType.DefenceItem2, defenceItem2Count),
            defenceButtons[2].Initialize(DefenceItemType.DefenceItem3, defenceItem3Count)
        );
    }
}