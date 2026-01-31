using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using DG.Tweening;

public class InGamePausePopup : MonoBehaviour
{
    [Header("Popup")]
    [SerializeField] private GameObject popup;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Buttons")]
    [SerializeField] private Button settingButton;
    [SerializeField] private Button goToMenuButton;
    [SerializeField] private Button resumeButton;

    [Header("References")]
    [SerializeField] private SettingPopup settingPopup;

    private bool isPaused = false;

    private void Start()
    {
        settingButton.onClick.AddListener(OnClickSetting);
        goToMenuButton.onClick.AddListener(OnClickGoToMenu);
        resumeButton.onClick.AddListener(OnClickResume);

        popup.SetActive(false);
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
        isPaused = true;
        popup.SetActive(true);
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = true;

        DisablePlayerInput();

        canvasGroup.DOFade(1f, 0.25f);
        popup.transform.localScale = Vector3.one * 0.8f;
        popup.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
    }

    private void Resume()
    {
        isPaused = false;
        canvasGroup.blocksRaycasts = false;

        EnablePlayerInput();

        canvasGroup.DOFade(0f, 0.2f).OnComplete(() =>
        {
            popup.SetActive(false);
        });

        popup.transform.DOScale(0.8f, 0.2f);
    }

    private void OnClickSetting()
    {
        AudioManager.PlaySfx(SoundID.ButtonClick);
        settingPopup.gameObject.SetActive(true);
    }

    private void OnClickResume()
    {
        AudioManager.PlaySfx(SoundID.ButtonClick);
        Resume();
    }

    private void OnClickGoToMenu()
    {
        AudioManager.PlaySfx(SoundID.ButtonClick);

        if (NetworkManager.Singleton == null || !NetworkManager.Singleton.IsListening)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName.MENU);
            return;
        }

        if (NetworkManager.Singleton.IsServer)
        {
            HandleHostGoToMenu();
        }
        else
        {
            HandleClientGoToMenu();
        }
    }

    private void HandleHostGoToMenu()
    {
        Debug.Log("[InGamePausePopup] Host leaving - shutting down entire lobby");

        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.Shutdown();
        }

        if (LobbyRelayManager.Instance != null)
        {
            LobbyRelayManager.Instance.CurrentLobby = null;
        }

        if (NetworkGameController.Instance != null)
        {
            Destroy(NetworkGameController.Instance.gameObject);
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName.MENU);
    }

    private void HandleClientGoToMenu()
    {
        Debug.Log("[InGamePausePopup] Client leaving lobby");

        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.Shutdown();
        }

        if (LobbyRelayManager.Instance != null)
        {
            LobbyRelayManager.Instance.CurrentLobby = null;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName.MENU);
    }

    private void DisablePlayerInput()
    {
        var players = FindObjectsOfType<InputReader>();
        foreach (var player in players)
        {
            if (player.IsOwner)
            {
                player.enabled = false;
            }
        }
    }

    private void EnablePlayerInput()
    {
        var players = FindObjectsOfType<InputReader>();
        foreach (var player in players)
        {
            if (player.IsOwner)
            {
                player.enabled = true;
            }
        }
    }
}
