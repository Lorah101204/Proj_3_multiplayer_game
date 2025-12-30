using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class LevelsPopup : MonoBehaviour
{
    [SerializeField] private Transform contentParent;
    [SerializeField] private LevelButton levelButtonPrefab;

    public static int SelectedLevelIndex = 0;

    private void OnEnable()
    {
        BuildLevelButtons();
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
            button.SetUp(i, isLocked, OnLevelSelected);
        }
    }

    private void OnLevelSelected(int index)
    {
        if (!NetworkManager.Singleton.IsHost)
        {
            Debug.Log("Only host can select level");
            return;
        }

        SelectedLevelIndex = index;
        NetworkManager.Singleton.SceneManager.LoadScene(SceneName.GAMEPLAY, LoadSceneMode.Single);
    }
}
