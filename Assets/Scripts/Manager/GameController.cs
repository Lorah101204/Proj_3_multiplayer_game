using UnityEngine;
using DesignPattern;
using System.Collections;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
    }
}
