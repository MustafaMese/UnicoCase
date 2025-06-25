using System;
using DG.Tweening;
using UnityEngine;

public class FinishPanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject failedPanel;
    [SerializeField] private GameObject successPanel;

    private Sequence _fadeSequence;

    public void ShowSuccessPanel(Action<bool> onComplete)
    {
        successPanel.SetActive(true);

        _fadeSequence?.Kill();
        _fadeSequence = DOTween.Sequence();
        _fadeSequence.Append(canvasGroup.DOFade(1, 0.5f))
            .OnComplete(() => { onComplete?.Invoke(true); });
    }

    public void ShowFailedPanel(Action<bool> onComplete)
    {
        failedPanel.SetActive(true);
        
        _fadeSequence?.Kill();
        _fadeSequence = DOTween.Sequence();
        _fadeSequence.Append(canvasGroup.DOFade(1, 0.5f))
            .OnComplete(() => { onComplete?.Invoke(false); });
    }

    private void OnDestroy()
    {
        _fadeSequence?.Kill();
    }
}