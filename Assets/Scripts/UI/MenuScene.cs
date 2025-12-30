using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class MenuScene : MonoBehaviour
{
    [SerializeField] private GameObject settingPopupPrefab;
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
        HostJoinGO.SetActive(true);
    }

    private void OnSettingButtonClicked()
    { 
    }
}
