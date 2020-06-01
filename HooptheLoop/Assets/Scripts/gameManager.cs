using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;


public class gameManager : MonoBehaviour
{
    [Header("Game State")]
    public bool gameStarted = false;
    public bool gameOver = false;
    public bool godMode = false;

    [Header("Ring & Shrinking")]
    public GameObject ring;
    public GameObject pointer;
    public float shrinkFactor = 0.01f;
    public float startingRadius = 1.5f;
    public float smallestRadius = 0.1f;

    [Header("Bar Spawn")]
    public GameObject spawnPoint;
    public SteamVR_Action_Boolean spawnTube;
    public SteamVR_Action_Boolean restart;
    public SteamVR_Input_Sources handType;
    public GameObject barParent;
    public GameObject barParentPrefab;
    private int[] numberOfTimes = new int[] { 0, 0, 0 };
    public GameObject[] barPrefabs;
    public int barLength = 100;
    private int currentNumber = 0;
    private int previousNumber = 0;
    private GameObject previousBar;


    [Header("Gameover objects to Enable/Disable")]
    public GameObject[] objectsToEnable;
    public GameObject[] objectsToDisable;

    


    private void Start()
    {
        


        EnableObjects(objectsToDisable);
        DisableObjects(objectsToEnable);
        
    }

    private void Update()
    {
        ring = GameObject.Find("Ring");
        pointer = GameObject.Find("Pointer");
        ring.SetActive(true);
        pointer.SetActive(true);


        if (gameStarted == true)
        {
            DisableObjects(objectsToDisable);
            DisableObjects(objectsToEnable);
            barParent.SetActive(true);
        }
        if (gameOver == true && godMode == false)
        {
            EnableObjects(objectsToEnable);
            DisableObjects(objectsToDisable);
            barParent.SetActive(false);
        }
        if (spawnTube.GetState(handType))
        {
            Debug.Log("trigger pressed");
            if (gameStarted == false)
            {
                instantiateTube();
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
        }
    }

    private void FixedUpdate()
    {
        if (ring.transform.localScale.x <= smallestRadius)
        {
            Debug.Log("smallest size");
        }
        else
        {
            ring.transform.localScale += new Vector3(-shrinkFactor/100f, -shrinkFactor / 100f, 0f);
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
    void instantiateTube()
    {
        previousBar = Instantiate(barPrefabs[0], spawnPoint.transform.position, Quaternion.identity);
        previousBar.transform.parent = barParent.transform;
        for (int i = 0; i < barLength; i++)
        {

            switch (previousNumber)
            {
                case 0:
                    currentNumber = Random.Range(0, 3);
                    previousBar = Instantiate(barPrefabs[currentNumber], new Vector3(previousBar.transform.position.x + 0.75f, previousBar.transform.position.y, previousBar.transform.position.z), Quaternion.identity);
                    numberOfTimes[currentNumber]++;
                    break;
                case 1:
                    currentNumber = Random.Range(0, 3);
                    if ((numberOfTimes[1] - numberOfTimes[2] >= 0) && currentNumber == 1)
                    {
                        Debug.Log("added down");
                        currentNumber = 2;
                    }
                    previousBar = Instantiate(barPrefabs[currentNumber], new Vector3(previousBar.transform.position.x + 0.75f, previousBar.transform.position.y + 0.5f, previousBar.transform.position.z), Quaternion.identity);
                    numberOfTimes[currentNumber]++;
                    break;
                case 2:
                    currentNumber = Random.Range(0, 3);
                    if ((numberOfTimes[2] - numberOfTimes[1] >= 0) && currentNumber == 2)
                    {
                        Debug.Log("added up");
                        currentNumber = 1;
                    }
                    previousBar = Instantiate(barPrefabs[currentNumber], new Vector3(previousBar.transform.position.x + 0.75f, previousBar.transform.position.y - 0.5f, previousBar.transform.position.z), Quaternion.identity);
                    numberOfTimes[currentNumber]++;
                    break;
            }
            previousBar.transform.parent = barParent.transform;
            previousNumber = currentNumber;
        }
    }
    void restartGame()
    {
        gameStarted = false;
        gameOver = false;
        EnableObjects(objectsToDisable);
        DisableObjects(objectsToEnable);
        Destroy(barParent);
        barParent = Instantiate(barParentPrefab, Vector3.zero, Quaternion.identity);
        previousNumber = 0;
        ring.transform.localScale = new Vector3(startingRadius, startingRadius, 0.1f);
    }
}

