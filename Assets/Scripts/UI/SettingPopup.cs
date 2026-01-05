using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using DesignPattern;
using TMPro;

public class SettingPopup : MonoBehaviour
{
    [Header("Popup")]
    [SerializeField] private GameObject popup;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("UI Elements")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button closeButton;

    [Header("Display Mode")]
    [SerializeField] private Button windowButton;
    [SerializeField] private Button fullscreenButton;

    private Resolution[] resolutions;

    private void Start()
    {
        InitResolutionDropdownFromManager();
        LoadAudioToUI();

        closeButton.onClick.AddListener(Hide);
        windowButton.onClick.AddListener(SetWindowMode);
        fullscreenButton.onClick.AddListener(SetFullscreenMode);
    }

    private void InitResolutionDropdownFromManager()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        var options = new List<string>();
        int currentIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option =
                $"{resolutions[i].width} x {resolutions[i].height} {resolutions[i].refreshRateRatio.value}Hz";

            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentIndex;
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener(OnResolutionSizeChanged);
    }

    private void OnResolutionSizeChanged(int index)
    {
        var res = resolutions[index];

        this.PostEvent(
            EventID.ResolutionChanged,
            new ResolutionData(res.width, res.height, Screen.fullScreen)
        );
    }


    private void SetWindowMode()
    {
        AudioManager.PlaySfx(SoundID.ButtonClick);
        var res = Screen.currentResolution;

        this.PostEvent(
            EventID.ResolutionChanged,
            new ResolutionData(res.width, res.height, false)
        );
    }

    private void SetFullscreenMode()
    {
        AudioManager.PlaySfx(SoundID.ButtonClick);
        var res = Screen.currentResolution;

        this.PostEvent(
            EventID.ResolutionChanged,
            new ResolutionData(res.width, res.height, true)
        );
    }

    // --- AUDIO PART ---

    private void LoadAudioToUI()
    {
        masterSlider.value = AudioManager.Instance.MasterVolume;
        musicSlider.value  = AudioManager.Instance.MusicVolume;
        sfxSlider.value    = AudioManager.Instance.SfxVolume;

        masterSlider.onValueChanged.AddListener(v => AudioManager.Instance.MasterVolume = v);
        musicSlider.onValueChanged.AddListener(v  => AudioManager.Instance.MusicVolume  = v);
        sfxSlider.onValueChanged.AddListener(v    => AudioManager.Instance.SfxVolume    = v);
    }



    public void Show()
    {
        popup.SetActive(true);
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = true;

        canvasGroup.DOFade(1f, 0.25f);
        popup.transform.localScale = Vector3.one * 0.8f;
        popup.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
    }

    public void Hide()
    {
        AudioManager.PlaySfx(SoundID.ButtonClick);
        canvasGroup.blocksRaycasts = false;

        canvasGroup.DOFade(0f, 0.2f).OnComplete(() =>
        {
            popup.SetActive(false);
        });

        popup.transform.DOScale(0.8f, 0.2f);
    }
}
