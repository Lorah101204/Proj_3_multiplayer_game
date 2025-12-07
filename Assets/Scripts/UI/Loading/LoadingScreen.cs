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

    [SerializeField]
    private float defaultLoadingStep = 0.2f;

    private bool isFirstLoading = true;
    private void Start()
    {
        EventDispatcher.Instance.RegisterListener(EventID.OnLoadingChangeProgress, (param) => OnLoadingChangeProgress((Tuple<float, float>)param));
    }

    private void OnLoadingChangeProgress(Tuple<float, float> param)
    {
        var percentage = param.Item1;
        var timeLoad = param.Item2 < defaultLoadingStep ? defaultLoadingStep : param.Item2;
        if (percentage == 100)
        {
            if (!this.gameObject.activeSelf)
            {
                Show();
            }
            loadingSlider.DOKill();
            loadingSlider.DOValue(percentage, timeLoad);
            Invoke(nameof(Hide), delayBeforeHiding);
        }
        else
        {
            if (!this.gameObject.activeSelf)
            {
                Show();
            }

            loadingSlider.DOKill();
            loadingSlider.DOValue(percentage, timeLoad);
        }
    }


    private void Hide()
    {
        canvas
            .DOFade(0, 0.25f)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                EventDispatcher.Instance.PostEvent(EventID.LoadingCompletedEvent, this);
            });
        isFirstLoading = false;
    }

    private void Show()
    {
        gameObject.SetActive(true);
        canvas.alpha = 1f;
        loadingSlider.value = 0;
    }
}


