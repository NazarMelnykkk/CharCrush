using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventHandler : MonoBehaviour
{
    public static GameEventHandler Instance { get; private set; }

    public GameEvents GameEvents;
    public GameInfoEvent GameInfoEvent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        GameEvents = new GameEvents();
        GameInfoEvent = new GameInfoEvent();
    }
}