using System.Threading.Tasks;
using CustomEvent;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DefenceButton : Button
{
    private TextMeshProUGUI _text;
    private CanvasGroup _canvasGroup;
    private int _count;
    private DefenceItemType _type;

    public Task Initialize(DefenceItemType type, int count)
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 1f;
        _canvasGroup.interactable = false;
        
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _text.text = count.ToString();
        _count = count;
        _type = type;
        
        interactable = count > 0;
        var code = type == DefenceItemType.DefenceItem1 ? "1" :
            type == DefenceItemType.DefenceItem2 ? "2" : "3";
        image.sprite = Resources.Load<Sprite>($"DefenceItems/{code}");
        
        GameManager.Instance.CustomEvent.AddCustomEventListener<SelectionFailed>(OnDefenceItemReturned);
        
        onClick.AddListener(() =>
        {
            if(_count <= 0)
            {
                interactable = false;
                return;
            }

            Add(-1);
            
            GameManager.Instance.CustomEvent.InvokeCustomEvent(
                new SelectionStarted()
                {
                    Type = _type
                });
        });

         return transform.DOScale(1, 0.5f)
            .OnComplete(() => _canvasGroup.interactable = interactable)
            .AsyncWaitForCompletion();
    }

    private void OnDefenceItemReturned(SelectionFailed e)
    {
        if (e.Type != _type) return;
        
        Add(1);
    }

    private void Add(int number)
    {
        _count += number;
        _text.text = _count.ToString();
        interactable = _count > 0;
    }
}