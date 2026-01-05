using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class MenuScene : MonoBehaviour
{
    [SerializeField] private SettingPopup settingPopup;
    [SerializeField] private GameObject HostJoinGO;
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingButton;

    private void OnEnable()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        settingButton.onClick.AddListener(OnSettingButtonClicked);
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveListener(OnPlayButtonClicked);
        settingButton.onClick.RemoveListener(OnSettingButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        AudioManager.PlaySfx(SoundID.ButtonClick);
        HostJoinGO.SetActive(true);
    }

    private void OnSettingButtonClicked()
    {
        AudioManager.PlaySfx(SoundID.ButtonClick);
        settingPopup.gameObject.SetActive(true);
        settingPopup.Show();
    }
}
