using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using DesignPattern;

public class LevelsPopup : MonoBehaviour
{
    [SerializeField] private Transform contentParent;
    [SerializeField] private LevelButton levelButtonPrefab;


    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.OnLevelButtonClicked, (obj) => OnLevelSelected((int)obj));
        BuildLevelButtons();
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.OnLevelButtonClicked, (obj) => OnLevelSelected((int)obj));
    }

    private void BuildLevelButtons()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        int currentUnlocked = PlayerPrefs.GetInt(GameConstant.CURRENT_LEVEL_INDEX, 0);
        int total = LevelManager.Instance.levelData.GetTotalLevels();

        for (int i = 0; i < total; i++)
        {
            LevelButton button = Instantiate(levelButtonPrefab, contentParent);
            bool isLocked = i > currentUnlocked;
            button.SetUp(i, isLocked);
        }
    }

    private void OnLevelSelected(int index)
    {
        if (!NetworkManager.Singleton.IsHost)
        {
            Debug.Log("Only host can select level");
            return;
        }
        NetworkGameController.Instance.SetCurrentLevel(index);
        NetworkManager.Singleton.SceneManager.LoadScene(SceneName.GAMEPLAY, LoadSceneMode.Single);
    }
}