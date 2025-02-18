﻿using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [Header("GameState")]
    public static GameManager instance = null;

    public enum GameState
    {
        Main = 0,
        Logo,
        Level
    }
    public GameState currentState;
    public GameState nextState;
    public int highScore;
 
    [Header("Level Transition")]
    public LevelTransitioner levelTrans;
    public float timeDelay = 1f;
    private float timeDelayClock;
    
    [Header("Scene Number")]
    public int sceneMain = 0;
    public int sceneLogo = 1;
    public int sceneLevel = 2;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        if(instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }
    private void Start()
    {
        timeDelayClock = timeDelay;  
    }
    public int GetHighScore()
    {
        return highScore;
    }
    public void SetHighScore(int set)
    {
        highScore = set;
    }

    private void Update()
    {
        if (currentState != nextState)
        {
            Time.timeScale = 1;
            switch (nextState)
            {
                case GameState.Main:
                    SceneManager.LoadScene(sceneMain);
                    currentState = nextState;
                    break;
                case GameState.Logo:
                    SceneManager.LoadScene(sceneLogo);
                    currentState = nextState;
                    nextState = GameState.Level;
                    break;
                case GameState.Level:
                    if(timeDelayClock <= 0f)
                    {
                        levelTrans.FadeToLevel(sceneLevel);
                        currentState = nextState;
                        break;
                    }
                    timeDelayClock -= Time.deltaTime;
                    break;
                default:
                    break;
            }
        }
    }
}
