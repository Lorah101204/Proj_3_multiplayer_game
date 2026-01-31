using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class HostJoinPopup : MonoBehaviour
{
    public Button buttonHost;
    public Button buttonJoin;
    public Button buttonConfirmJoin;
    public Button closeButton;
    public Button cancelJoinButton;

    public GameObject joinPanel;
    public TMP_InputField joinCodeInput;

    private void OnEnable()
    {
        buttonHost.onClick.AddListener(OnClickHost);
        buttonJoin.onClick.AddListener(OnClickShowJoinPanel);
        buttonConfirmJoin.onClick.AddListener(OnClickConfirmJoin);
        closeButton.onClick.AddListener(OnClickClose);
        cancelJoinButton.onClick.AddListener(OnClickCancelJoin);

        LobbyRelayManager.Instance.OnHostReady += LoadLobbyScene;
        LobbyRelayManager.Instance.OnClientReady += LoadLobbyScene;
    }

    private void OnDisable()
    {
        buttonHost.onClick.RemoveListener(OnClickHost);
        buttonJoin.onClick.RemoveListener(OnClickShowJoinPanel);
        buttonConfirmJoin.onClick.RemoveListener(OnClickConfirmJoin);
        closeButton.onClick.RemoveListener(OnClickClose);
        cancelJoinButton.onClick.RemoveListener(OnClickCancelJoin);

        LobbyRelayManager.Instance.OnHostReady -= LoadLobbyScene;
        LobbyRelayManager.Instance.OnClientReady -= LoadLobbyScene;
    }

    async void OnClickHost()
    {
        AudioManager.PlaySfx(SoundID.ButtonClick);
        await LobbyRelayManager.Instance.CreateLobbyAndRelay();
    }

    void OnClickShowJoinPanel()
    {
        AudioManager.PlaySfx(SoundID.ButtonClick);
        joinPanel.SetActive(true);
    }

    async void OnClickConfirmJoin()
    {
        AudioManager.PlaySfx(SoundID.ButtonClick);
        string code = joinCodeInput.text.Trim();
        if (string.IsNullOrEmpty(code)) return;

        await LobbyRelayManager.Instance.JoinLobbyAndRelay(code);
    }

    void LoadLobbyScene()
    {
        NetworkManager.Singleton.SceneManager.LoadScene(SceneName.LOBBY, LoadSceneMode.Single);
    }
    
    void OnClickClose()
    {
        AudioManager.PlaySfx(SoundID.ButtonClick);
        gameObject.SetActive(false);
    }

    void OnClickCancelJoin()
    {
        AudioManager.PlaySfx(SoundID.ButtonClick);
        joinPanel.SetActive(false);
    }
}