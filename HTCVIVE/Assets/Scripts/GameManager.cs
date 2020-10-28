using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using HTC.UnityPlugin.Vive;

public class GameManager : MonoBehaviour
{
    [Header("Game State")]
    public bool gameStarted = false;
    public bool gameOver = false;
    public bool godMode = false;

    [Header("Ring & Shrinking")]
    public GameObject ring;
    public VivePoseTracker ringPoseTracker;
    public GameObject positionTracker;
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
    public float timeDelay;
    private float timeDelayClock;

    [Header("Score Keep")]
    public float gameTime = 0.0f;
    public Text timeText;
    public Text gameOverTimeText;


    [Header("Gameover objects to Enable/Disable")]
    public GameObject[] objectsToEnable;
    public GameObject[] objectsToDisable;
    private int gameOverRunNumber = 0;
    private int gameStartRunNumber = 0;



    private void Start()
    {
        EnableObjects(objectsToDisable);
        DisableObjects(objectsToEnable);
        previousBar = Instantiate(barPrefabs[0], spawnPoint.transform.position, Quaternion.identity);
        previousBar.transform.parent = barParent.transform;
        for (int i = 0; i <= 10; i++)
        {
            previousBar = Instantiate(barPrefabs[0], new Vector3(previousBar.transform.position.x + 0.75f, previousBar.transform.position.y, previousBar.transform.position.z), Quaternion.identity);
            previousBar.transform.parent = barParent.transform;
        }
    }

    private void Update()
    {
        
        ring = GameObject.Find("Hoop");
        ring.SetActive(true);
        timeDelayClock += Time.deltaTime;
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
                ringPoseTracker.enabled = true;
                ringPoseTracker.posOffset = new Vector3(positionTracker.transform.position.x, positionTracker.transform.position.y - 1.2f, positionTracker.transform.position.z - 0.1f);
                DisableObjects(objectsToDisable);
                DisableObjects(objectsToEnable);
                barParent.SetActive(true);
            }
            gameStartRunNumber++;
            if(timeDelayClock >= timeDelay)
            {
                InstantiateTube(CheckBars());
                timeDelayClock = 0f;
            }
            
        }
        else
        {
            if (timeDelayClock >= timeDelay)
            {
                InstantiateTube(0);
                timeDelayClock = 0f;
            }

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
                previousBar = Instantiate(barPrefabs[currentNumber], new Vector3(previousBar.transform.position.x + 0.75f, previousBar.transform.position.y + 0.5f, previousBar.transform.position.z), Quaternion.identity);
                numberOfTimes[currentNumber]++;
                break;
            case 2:
                previousBar = Instantiate(barPrefabs[currentNumber], new Vector3(previousBar.transform.position.x + 0.75f, previousBar.transform.position.y - 0.5f, previousBar.transform.position.z), Quaternion.identity);
                numberOfTimes[currentNumber]++;
                break;
        }
        previousBar.transform.parent = barParent.transform;
        previousNumber = currentNumber;
        
    }

    int CheckBars()
    {
        if (numberOfTimes[1] - numberOfTimes[2] >= 1)
        {
            Debug.Log("added down");
            return 2;
        }
        else if (numberOfTimes[2] - numberOfTimes[1] >= 1)
        {
            Debug.Log("added up");
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
}
