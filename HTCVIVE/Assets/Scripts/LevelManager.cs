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
    public GameObject startText;
    public GameObject hoopGuideText;
    public Animator ringAnim;
    public float distanceAllowence = 0.5f;
    public float angleAllowence = 10f;
    public GameObject ring;
    public GameObject ringScalePivot;
    public int shrinkFrequencey = 3;
    public float shrinkFactor = 0.01f;
    public float startingRadius = 1.5f;
    public float smallestRadius = 0.1f;

    [Header("Bar Spawn")]
    public Material[] barColors;
    public GameObject hoopGuide;
    public GameObject spawnPoint;
    public GameObject barParent;
    private int[] numberOfTimes = { 0, 0, 0 };
    public GameObject[] barPrefabs;
    public int barLength = 100;
    private int previousNumber = 0;
    private GameObject previousBar;

    [Header("Score Keep")]
    public int score;
    public Text timeText;
    public Text gameOverTimeText;
    public Text highScoreText;


    [Header("Gameover objects to Enable/Disable")]
    public GameObject[] objectsToEnable;
    public GameObject[] objectsToDisable;

    [Header("Sound Effect")]
    public AudioSource gainScoreSound;
    public AudioSource pauseSound;
    public AudioSource loseSound;
    public AudioSource backgroundMusic;

    // Start is called before the first frame update
    void Start()
    {
        EnableObjects(objectsToDisable);
        DisableObjects(objectsToEnable);
        ring = GameObject.Find("Hoop");
        ring.SetActive(true);
    }

    private void Update()
    {
        //pausedText.SetActive(gamePaused && gameStarted && !gameOver);
        if (gameStarted)
        {
           

            hoopGuideText.SetActive(false);
            startText.SetActive(false);
            timeText.gameObject.SetActive(true);
            if(!gameOver)
            {
                timeText.text = score.ToString();
                
            }
            else
            {
                gamePaused = false;
            }
            if (isFirstGame)
            {
                pauseSound.Play();
                DisableObjects(objectsToDisable);
                DisableObjects(objectsToEnable);
                barParent.SetActive(true);
                previousBar = Instantiate(barPrefabs[0], spawnPoint.transform.position, Quaternion.identity);
                previousBar.transform.GetChild(0).GetComponent<Renderer>().material = barColors[barColors.Length - 1];
                previousBar.transform.parent = barParent.transform;
                for (int i = 0; i <= barLength; i++)
                {
                    InstantiateTube(CheckBars());
                    previousBar.transform.GetChild(0).GetComponent<Renderer>().material = barColors[i % barColors.Length];
                }
                isFirstGame = false;
            }


            if (gamePaused)
            {
                checkGuide(distanceAllowence, angleAllowence);
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
        else
        {
            checkGuide(distanceAllowence, angleAllowence);
        }
        if (gameOver && !godMode)
        {
            if (score > GameManager.instance.GetHighScore())
            {
                GameManager.instance.SetHighScore(score); 
            }
            highScoreText.text = "High Score: " + GameManager.instance.GetHighScore().ToString();
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
                    if(checkGuide(1f, 10f))
                    {
                        gamePaused = false;
                        pauseSound.Play();
                    }
                }
                else
                {
                    gamePaused = true;
                    pauseSound.Play();
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
            if (Vector3.Distance(ring.transform.position, hoopGuide.transform.position) < distanceAllowence == true)
            {
                if(Quaternion.Angle(ring.transform.rotation, hoopGuide.transform.rotation) < angleAllowence == true)
                {
                    setColor(hoopGuide.transform.GetChild(0), Color.green);
                    if (!gamePaused)
                    {
                        startText.SetActive(true);
                        hoopGuideText.SetActive(false);
                    }
                    ringAnim.Play("Idle");
                    return true;
                }
                setColor(hoopGuide.transform.GetChild(0), Color.yellow);
                if (!gamePaused)
                {
                    startText.SetActive(false);
                    hoopGuideText.SetActive(true);
                }
                ringAnim.Play("HoopGuide");
                return false;
            }
            setColor(hoopGuide.transform.GetChild(0), Color.yellow);
            if (!gamePaused)
            {
                startText.SetActive(false);
                hoopGuideText.SetActive(true);
            }
            ringAnim.Play("HoopGuide");
            return false;
            
        }

        void setColor(Transform obj, Color color)
        {
            color.a = 0.45f;
            Material mat = obj.GetComponent<Renderer>().material;
            mat.SetColor("_Color", color);
  
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

    void OnApplicationFocus(bool hasFocus)
    {
        Debug.Log(hasFocus);
        if (!hasFocus)
        {
            gamePaused = true;
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
        backgroundMusic.Play();
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

    public void addScore(int score)
    {
        this.score += score;
        gainScoreSound.Play();
        if (this.score % shrinkFrequencey == 0)
        {
            if (ringScalePivot.transform.localScale.x <= smallestRadius)
            {
                Debug.Log("smallest size");
            }
            else
            {
                ringScalePivot.transform.localScale += new Vector3(-shrinkFactor, -shrinkFactor / 2f, -shrinkFactor);
            }
        }
        
    }

    public void playLoseSound()
    {
        loseSound.Play();
        backgroundMusic.Stop();
    }
}
