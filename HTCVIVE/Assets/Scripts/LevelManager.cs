using UnityEngine;
using UnityEngine.UI;
using HTC.UnityPlugin.Vive;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    [Header("Game State")]
    public bool gameStarted = false;
    public bool gameOver = false;
    public bool godMode = false;

    private bool isFirstGameOver = true;
    private bool isFirstGame = true;

    [Header("Pause")]
    public bool gamePaused = false;
    private bool isPauseFirst = true;
    public GameObject hoopPauseSphere;
    public GameObject hoopGuidePauseSphere;

    public GameObject pausedText;
    public Vector3 positionOffset;

    [Header("Ring & Shrinking")]
    public GameObject ring;
    public GameObject ringScalePivot;
    public float ringTimer = 2f;
    private float ringTimerClock;
    public float shrinkFactor = 0.01f;
    public float startingRadius = 1.5f;
    public float smallestRadius = 0.1f;

    [Header("Bar Spawn")]
    public GameObject hoopGuide;
    public GameObject spawnPoint;
    public GameObject barParent;
    private int[] numberOfTimes = { 0, 0, 0 };
    public GameObject[] barPrefabs;
    public int barLength = 100;
    private int previousNumber = 0;
    private GameObject previousBar;

    [Header("Score Keep")]
    public float gameTime = 0.0f;
    public Text timeText;
    public Text gameOverTimeText;
    public Text highScoreText;


    [Header("Gameover objects to Enable/Disable")]
    public GameObject[] objectsToEnable;
    public GameObject[] objectsToDisable;
    // Start is called before the first frame update
    void Start()
    {
        EnableObjects(objectsToDisable);
        DisableObjects(objectsToEnable);
        ring = GameObject.Find("Hoop");
        ring.SetActive(true);
        ringTimerClock = ringTimer;
    }

    private void Update()
    {
        pausedText.SetActive(gamePaused && gameStarted && !gameOver);
        if (gameStarted)
        {
            timeText.gameObject.SetActive(true);
            if(!gameOver)
            {
                gameTime += Time.deltaTime;
                timeText.text = gameTime.ToString("F2") + "sec";
                ringTimerClock -= Time.deltaTime;
                if (ringTimerClock <= 0)
                {
                    if (ringScalePivot.transform.localScale.x <= smallestRadius)
                    {
                        Debug.Log("smallest size");
                    }
                    else
                    {
                        ringScalePivot.transform.localScale += new Vector3(-shrinkFactor, -shrinkFactor / 2f, -shrinkFactor);
                    }
                    ringTimerClock = ringTimer;
                }
            }
            else
            {
                gamePaused = false;
            }
            if (isFirstGame)
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
                isFirstGame = false;
            }


            if (gamePaused)
            {
                if (isPauseFirst)
                {
                    hoopGuide.transform.position = ring.transform.position + positionOffset;
                    hoopGuide.transform.rotation = ring.transform.rotation;
                    
                    isPauseFirst = false;
                }
            }
            else
            {
                isPauseFirst = true;
            }
            
            Time.timeScale = gamePaused ? 0 : 1;
            hoopGuide.SetActive(gamePaused);
        }
        if (gameOver && !godMode)
        {
            if (gameTime > GameManager.instance.GetHighScore())
            {
                GameManager.instance.SetHighScore(gameTime); 
            }
            highScoreText.text = "High Score: " + GameManager.instance.GetHighScore().ToString("F2");
            //timeText.gameObject.SetActive(false);
            //gameOverTimeText.text = gameTime.ToString("F2") + "sec";
            if (isFirstGameOver)
            {
                EnableObjects(objectsToEnable);
                DisableObjects(objectsToDisable);
                barParent.SetActive(false);
                isFirstGameOver = false;
            }
        }

        //Check for A
        if (ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.AKey))
        {
            if (gameStarted)
            {
                if (gamePaused)
                {
                    gamePaused = !checkGuide(1f, 10f);
                }
                else
                {
                    gamePaused = true;
                }
            }
        }

        //Check for trigger
        if (ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Trigger))
        {
            if (checkGuide(1f, 10f))
            {
                gameStarted = true;
            }
            Debug.Log("trigger pressed");
            ViveInput.TriggerHapticPulseEx(HandRole.RightHand);
        }


        //Check for grip
        if (ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Grip))
        {
            Debug.Log("grip pressed");
            if (gameOver == true)
            {
                RestartGame();
            }
        }

        
        bool checkGuide(float distanceAllowence, float angleAllowence)
        {
            Debug.Log(Vector3.Distance(ring.transform.position, hoopGuide.transform.position));
            Debug.Log(Quaternion.Angle(ring.transform.rotation, hoopGuide.transform.rotation));
            return Vector3.Distance(ring.transform.position, hoopGuide.transform.position) < distanceAllowence && Quaternion.Angle(ring.transform.rotation, hoopGuide.transform.rotation) < angleAllowence;
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
            return 2;
        }
        else if (numberOfTimes[2] - numberOfTimes[1] >= 2)
        {
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
