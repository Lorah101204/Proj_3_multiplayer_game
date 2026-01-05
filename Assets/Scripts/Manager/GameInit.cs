using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameInit : MonoBehaviour
{
    public static GameInit Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;
        ResolutionManager.Instance.ApplySavedResolution();
    }

    public virtual void Start()
    {
        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(SceneName.MENU);
        yield return new WaitForSeconds(0.2f);
        AudioManager.PlayMusic(SoundID.BGMMusic, true);
    }
}
