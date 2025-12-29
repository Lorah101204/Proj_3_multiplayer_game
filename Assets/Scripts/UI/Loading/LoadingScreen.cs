using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;
using DesignPattern;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] CanvasGroup canvas;
    [SerializeField] Slider loadingSlider;

    [SerializeField] private float delayBeforeHiding = 0.5f;
    [SerializeField] private float defaultLoadingStep = 0.2f;

    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.OnLoadingChangeProgress, OnProgressEvent);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.OnLoadingChangeProgress, OnProgressEvent);
    }

    private void OnProgressEvent(object param)
    {
        if (param is not Tuple<float, float> t) return;
        OnLoadingChangeProgress(t);
    }

    private void OnLoadingChangeProgress(Tuple<float, float> param)
    {
        float percentage = param.Item1;
        float timeLoad = Mathf.Max(param.Item2, defaultLoadingStep);

        if (!gameObject.activeSelf) Show();

        loadingSlider.DOKill();
        loadingSlider.DOValue(percentage, timeLoad);

        if (percentage >= 100f)
        {
            Invoke(nameof(Hide), delayBeforeHiding);
        }
    }

    private void Hide()
    {
        canvas.DOFade(0, 0.25f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            EventDispatcher.Instance.PostEvent(EventID.LoadingCompletedEvent, this);
        });
    }

    private void Show()
    {
        gameObject.SetActive(true);
        canvas.alpha = 1f;
        loadingSlider.value = 0;
    }
}
