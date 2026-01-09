using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DesignPattern;

public class LevelButton : MonoBehaviour
{
    public TextMeshProUGUI levelIndex;
    [SerializeField] private Button button;
    [SerializeField] private Image lockImage;

    private int levelIndexValue;

    private void OnEnable()
    {
        button.onClick.AddListener(OnLevelButtonClicked);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnLevelButtonClicked);
    }

    public void SetUp(int index, bool isLocked)
    {
        levelIndexValue = index;

        levelIndex.text = (index + 1).ToString();
        levelIndex.gameObject.SetActive(!isLocked);
        lockImage.gameObject.SetActive(isLocked);

        button.interactable = !isLocked;
    }

    private void OnLevelButtonClicked()
    {
        AudioManager.PlaySfx(SoundID.ButtonClick);
        EventDispatcher.Instance.PostEvent(EventID.OnLevelButtonClicked, levelIndexValue);
    }
}
