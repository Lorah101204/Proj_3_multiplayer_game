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
    }

    private void Start()
    {
        EventDispatcher.Instance.RegisterListener(EventID.LoadingCompletedEvent, OnLoadingCompleted);
    }

    private void OnLoadingCompleted(object param)
    {
        string loadedScene = param as string;
        if (loadedScene == SceneName.GAMEPLAY)
        {
        }
        IsPaused = false;
    }

}
