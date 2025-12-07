using UnityEngine;
using System.Collections;
using DesignPattern;

public class SceneManager : Singleton<SceneManager>
{
    private Coroutine loadingCoroutine;

    public void _LoadScene(string sceneName)
    {
        loadingCoroutine = StartCoroutine(_LoadSceneAsync(sceneName));
    }

    private IEnumerator _LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f) * 100f;
            float timeToLoad = 0.1f;

            EventDispatcher.Instance.PostEvent(EventID.OnLoadingChangeProgress, new System.Tuple<float, float>(progress, timeToLoad));

            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public static void LoadScene(string sceneName)
    {
        Instance._LoadScene(sceneName);
    }
}

public static class SceneName
{
    public const string MENU = "Menu";
    public const string LOBBY = "Lobby";
    public const string GAMEPLAY = "Gameplay";
}