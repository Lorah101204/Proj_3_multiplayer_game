using UnityEngine;
using System.Collections;
public class GameInit : MonoBehaviour
{
    public static GameInit Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;
    }

    public virtual void Start()
    {
        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(SceneName.MENU);
    }
}
