using UnityEngine;
using DesignPattern;

public class ResolutionManager : MonoBehaviour
{
    public static ResolutionManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            EventDispatcher.Instance.RegisterListener(EventID.ResolutionChanged, OnResolutionEvent);
        }
        else Destroy(gameObject);
    }

    private void OnDestroy()
    {
        EventDispatcher.Instance.RemoveListener(EventID.ResolutionChanged, OnResolutionEvent);
    }

    void OnResolutionEvent(object param)
    {
        var data = (ResolutionData)param;
        SetResolution(data.width, data.height, data.fullscreen);
    }

    public void SetResolution(int width, int height, bool fullscreen)
    {
        Screen.SetResolution(width, height, fullscreen);
        Debug.Log($"Resolution changed to: {width}x{height}, Fullscreen: {fullscreen}");
        PlayerPrefs.SetInt(GameConstant.SAVE_WIDTH, width);
        PlayerPrefs.SetInt(GameConstant.SAVE_HEIGHT, height);
        PlayerPrefs.SetInt(GameConstant.SAVE_FULLSCREEN, fullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ApplySavedResolution()
    {
        int width = PlayerPrefs.GetInt(GameConstant.SAVE_WIDTH, Screen.currentResolution.width);
        int height = PlayerPrefs.GetInt(GameConstant.SAVE_HEIGHT, Screen.currentResolution.height);
        bool fullscreen = PlayerPrefs.GetInt(GameConstant.SAVE_FULLSCREEN, 1) == 1;
        Debug.Log($"Applying saved resolution: {width}x{height}, Fullscreen: {fullscreen}");
        Screen.SetResolution(width, height, fullscreen);
    }
}

[System.Serializable]
public struct ResolutionData
{
    public int width;
    public int height;
    public bool fullscreen;

    public ResolutionData(int w, int h, bool f)
    {
        width = w; height = h; fullscreen = f;
    }
}
