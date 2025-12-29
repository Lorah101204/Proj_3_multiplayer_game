using UnityEngine;
using DesignPattern;
using Unity.Netcode;
using System.Collections;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public static bool IsPaused;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        EventDispatcher.Instance.RegisterListener(EventID.LoadingCompletedEvent, OnLoadingCompleted);
    }

    private void OnLoadingCompleted(object param)
    {
        string loadedScene = param as string;

        if (loadedScene == SceneName.LOBBY)
        {
            Debug.Log("Lobby loaded → StartHost()");
            StartCoroutine(StartHost());
        }

        IsPaused = false;
    }

    private IEnumerator StartHost()
    {
        yield return new WaitForSeconds(1f);
        NetworkManager.Singleton.StartHost();
    }
}
