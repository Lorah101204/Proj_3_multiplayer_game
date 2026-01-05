using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    public TextMeshProUGUI levelIndex;
    [SerializeField] private Button button;
    [SerializeField] private Image lockImage;

    private int levelIndexValue;
    private System.Action<int> onClick;

    private void OnEnable()
    {
        button.onClick.AddListener(OnLevelButtonClicked);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnLevelButtonClicked);
    }

    public void SetUp(int index, bool isLocked, System.Action<int> onClick)
    {
        levelIndexValue = index;
        this.onClick = onClick;

        levelIndex.text = (index + 1).ToString();
        levelIndex.gameObject.SetActive(!isLocked);
        lockImage.gameObject.SetActive(isLocked);

        button.interactable = !isLocked;
    }

    private void OnLevelButtonClicked()
    {
        AudioManager.PlaySfx(SoundID.ButtonClick);
        onClick?.Invoke(levelIndexValue);
    }
}
