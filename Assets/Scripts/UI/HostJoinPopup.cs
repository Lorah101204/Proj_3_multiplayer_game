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

    public GameObject joinPanel;
    public TMP_InputField joinCodeInput;

    private void OnEnable()
    {
        buttonHost.onClick.AddListener(OnClickHost);
        buttonJoin.onClick.AddListener(OnClickShowJoinPanel);
        buttonConfirmJoin.onClick.AddListener(OnClickConfirmJoin);

        LobbyRelayManager.Instance.OnHostReady   += LoadLobbyScene;
        LobbyRelayManager.Instance.OnClientReady += LoadLobbyScene;
    }

    private void OnDisable()
    {
        buttonHost.onClick.RemoveListener(OnClickHost);
        buttonJoin.onClick.RemoveListener(OnClickShowJoinPanel);
        buttonConfirmJoin.onClick.RemoveListener(OnClickConfirmJoin);

        LobbyRelayManager.Instance.OnHostReady   -= LoadLobbyScene;
        LobbyRelayManager.Instance.OnClientReady -= LoadLobbyScene;
    }

    async void OnClickHost()
    {
        await LobbyRelayManager.Instance.CreateLobbyAndRelay();
    }

    void OnClickShowJoinPanel()
    {
        joinPanel.SetActive(true);
    }

    async void OnClickConfirmJoin()
    {
        string code = joinCodeInput.text.Trim();
        if (string.IsNullOrEmpty(code)) return;

        await LobbyRelayManager.Instance.JoinLobbyAndRelay(code);
    }

    void LoadLobbyScene()
    {
        NetworkManager.Singleton.SceneManager.LoadScene(SceneName.LOBBY, LoadSceneMode.Single);
    }
}
