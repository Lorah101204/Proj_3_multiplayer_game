using UnityEngine;
using DesignPattern;
using System.Collections;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [SerializeField] private InGamePausePopup inGamePausePopup;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inGamePausePopup != null)
            {
                inGamePausePopup.TogglePause();
            }
        }
    }
}
