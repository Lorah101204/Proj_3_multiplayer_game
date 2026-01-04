using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingPopup : MonoBehaviour
{
    [Header("Popup")]
    [SerializeField] private GameObject popup;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button closeButton;

    [Header("UI Elements")]
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private Resolution[] resolutions;

    private void Start()
    {
        InitResolutionOptions();
        LoadAudioToUI();
    }

    private void OnEnable()
    {
        closeButton.onClick.AddListener(Hide);
    }
    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(Hide);
    }

    // ------------------ RESOLUTION ------------------

    private void InitResolutionOptions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        var options = new System.Collections.Generic.List<string>();
        int currentIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = $"{resolutions[i].width} x {resolutions[i].height} {resolutions[i].refreshRateRatio.value}Hz";
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

        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
    }

    private void OnResolutionChanged(int index)
    {
        var res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode, res.refreshRateRatio);
    }

    // ------------------ AUDIO ------------------

    private void LoadAudioToUI()
    {
        masterSlider.value = AudioManager.Instance.MasterVolume;
        musicSlider.value  = AudioManager.Instance.MusicVolume;
        sfxSlider.value    = AudioManager.Instance.SfxVolume;

        masterSlider.onValueChanged.AddListener(v => AudioManager.Instance.MasterVolume = v);
        musicSlider.onValueChanged.AddListener(v  => AudioManager.Instance.MusicVolume  = v);
        sfxSlider.onValueChanged.AddListener(v    => AudioManager.Instance.SfxVolume    = v);
    }

    // ------------------ POPUP ANIMATION ------------------

    public void Show()
    {
        popup.SetActive(true);
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = true;

        canvasGroup.DOFade(1f, 0.25f).SetEase(Ease.OutQuad);
        popup.transform.localScale = Vector3.one * 0.8f;
        popup.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
    }

    public void Hide()
    {
        canvasGroup.blocksRaycasts = false;

        canvasGroup.DOFade(0f, 0.2f).OnComplete(() =>
        {
            popup.SetActive(false);
        });

        popup.transform.DOScale(0.8f, 0.2f).SetEase(Ease.InQuad);
    }
}
