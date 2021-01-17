using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using HTC.UnityPlugin.Vive;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [Header("Game State")]
    public bool gameStarted = false;
    public bool gameOver = false;
    public bool godMode = false;
    public enum GameState
    {
        Logo = 0,
        Level
    }
    private GameState currentState;
    public GameState nextState;
    public LevelTransitioner levelTrans;
    public float timeDelay = 1f;
    private float timeDelayClock;
    [Header("Scenes")]
    public int sceneLogo = 0;
    public int sceneLevel = 1;

    [Header("Ring & Shrinking")]
    public GameObject ring;
    public float shrinkFactor = 0.01f;
    public float startingRadius = 1.5f;
    public float smallestRadius = 0.1f;

    [Header("Bar Spawn")]
    public GameObject spawnPoint;
    public GameObject barParent;
    public GameObject barParentPrefab;
    private int[] numberOfTimes = new int[] { 0, 0, 0 };
    public GameObject[] barPrefabs;
    public int barLength = 100;
    private int previousNumber = 0;
    private GameObject previousBar;

    [Header("Score Keep")]
    public float gameTime = 0.0f;
    public Text timeText;
    public Text gameOverTimeText;


    [Header("Gameover objects to Enable/Disable")]
    public GameObject[] objectsToEnable;
    public GameObject[] objectsToDisable;
    private int gameOverRunNumber = 0;
    private int gameStartRunNumber = 0;



    private void Awake()
    {
        DontDestroyOnLoad(this);
        instance = this;
    }
    private void Start()
    {
        timeDelayClock = timeDelay;
        currentState = GameState.Logo;
        nextState = GameState.Level;
        EnableObjects(objectsToDisable);
        DisableObjects(objectsToEnable);
    }

    private void Update()
    {
        timeDelayClock -= Time.deltaTime;
        if (currentState != nextState)
        {
            switch (nextState)
            {
                case GameState.Logo:
                    SceneManager.LoadScene(sceneLogo);
                    currentState = nextState;
                    break;
                case GameState.Level:
                    while (timeDelayClock >= 0f)
                    {
                        timeDelayClock -= Time.deltaTime;
                    }
                    levelTrans.FadeToLevel(sceneLevel);
                    currentState = nextState;
                    break;
                default:
                    break;
            }
        }
        ring = GameObject.Find("Hoop");
        ring.SetActive(true);
        if (gameStarted == true)
        {
            
            //timeText.gameObject.SetActive(true);

            //if(gameOver != true)
            //{
            //    gameTime += Time.deltaTime;
            //    timeText.text = gameTime.ToString("F2") + "sec";
            //}
            if (gameStartRunNumber < 1)
            {
                DisableObjects(objectsToDisable);
                DisableObjects(objectsToEnable);
                barParent.SetActive(true);
                previousBar = Instantiate(barPrefabs[0], spawnPoint.transform.position, Quaternion.identity);
                previousBar.transform.parent = barParent.transform;
                for (int i = 0; i <= barLength; i++)
                {
                    InstantiateTube(CheckBars());
                    previousBar.transform.parent = barParent.transform;
                }
            }
            gameStartRunNumber++;

            


        }
        if (gameOver == true && godMode == false)
        {
            //timeText.gameObject.SetActive(false);
            //gameOverTimeText.text = gameTime.ToString("F2") + "sec";
            if (gameOverRunNumber < 1)
            {
                EnableObjects(objectsToEnable);
                DisableObjects(objectsToDisable);
                barParent.SetActive(false);
            }
            gameOverRunNumber++;
        }

        if (ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Trigger))
        {
            Debug.Log("trigger pressed");
            gameStarted = true;
            /*ViveInput.TriggerHapticPulse(HandRole.RightHand);
            if (gameStarted == false)
            {
                InstantiateTube();
                
            }*/
        }
        if (ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Grip))
        {
            Debug.Log("grip pressed");
            if (gameOver == true)
            {
                RestartGame();
            }
        }

         /*   if (spawnTube.GetState(handType))
        {
            Debug.Log("trigger pressed");
            if (gameStarted == false)
            {
                InstantiateTube();
                gameStarted = true;
            }
        }
        if (restart.GetState(handType))
        {
            Debug.Log("grip pressed");
            if (gameOver == true)
            {
                restartGame();
            }
        }*/
    }

    private void FixedUpdate()
    {
        if (ring.transform.localScale.x <= smallestRadius)
        {
            Debug.Log("smallest size");
        }
        else
        {
            ring.transform.localScale += new Vector3(-shrinkFactor / 100f, -shrinkFactor / 100f, 0f);
        }
    }
    public void EnableObjects(GameObject[] objects)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(true);
        }
    }
    public void DisableObjects(GameObject[] objects)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(false);
        }
    }
    void InstantiateTube(int currentNumber)
    {
        switch (previousNumber)
        {
            case 0:
                previousBar = Instantiate(barPrefabs[currentNumber], new Vector3(previousBar.transform.position.x + 0.75f, previousBar.transform.position.y, previousBar.transform.position.z), Quaternion.identity);
                numberOfTimes[currentNumber]++;
                break;
            case 1:
                previousBar = Instantiate(barPrefabs[currentNumber], new Vector3(previousBar.transform.position.x + 0.75f, previousBar.transform.position.y + 0.4f, previousBar.transform.position.z), Quaternion.identity);
                numberOfTimes[currentNumber]++;
                break;
            case 2:
                previousBar = Instantiate(barPrefabs[currentNumber], new Vector3(previousBar.transform.position.x + 0.75f, previousBar.transform.position.y - 0.4f, previousBar.transform.position.z), Quaternion.identity);
                numberOfTimes[currentNumber]++;
                break;
        }
        previousBar.transform.parent = barParent.transform;
        previousNumber = currentNumber;

    }

    int CheckBars()
    {
        if (numberOfTimes[1] - numberOfTimes[2] >= 2)
        {
            Debug.Log("added down" + (numberOfTimes[1] - numberOfTimes[2]));
            return 2;
        }
        else if (numberOfTimes[2] - numberOfTimes[1] >= 2)
        {
            Debug.Log("added up" + +(numberOfTimes[1] - numberOfTimes[2]));
            return 1;
        }
        else
        {
            return Random.Range(0, 3);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadLobby()
    {
        SceneManager.LoadScene(0);
    }
    public void setGameState(int stateNum, bool setBool)
    {
        switch (stateNum)
        {
            case 0:
                gameStarted = setBool;
                break;
            case 1:
                gameOver = setBool;
                break;
            default:
                Debug.Log("SetGameState exception");
                break;
        }
    }
}
