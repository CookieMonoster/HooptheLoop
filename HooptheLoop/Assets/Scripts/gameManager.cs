using UnityEngine;
using UnityEngine.UI;
using Valve.VR;


public class gameManager : MonoBehaviour
{
    [Header("Game State")]
    public bool gameStarted = false;
    public bool gameOver = false;
    public bool godMode = false;

    [Header("Ring & Shrinking")]
    public GameObject ring;
    public float shrinkFactor = 0.01f;
    public float smallestRadius = 0.1f;

    [Header("Bar Spawn")]
    public GameObject spawnPoint;
    public SteamVR_Action_Boolean spawnTube;
    public SteamVR_Input_Sources handType;
    public GameObject barParent;
    public int[] numberOfTimes = new int[] { 0, 0, 0 };
    public GameObject[] barPrefabs;

    [Header("Other")]
    public Canvas gameOverCanvas;

    private int currentNumber = 0;
    private int previousNumber = 0;
    
    private int barLength = 100;
    
    private int currentBar = 0;
    private GameObject previousBar;


    
    private void Start()
    {
        gameOverCanvas.gameObject.SetActive(false);
        spawnTube.AddOnStateDownListener(TriggerDown, handType);
    }

    private void Update()
    {
        if (gameOver == true && godMode == false)
        {
            gameOverCanvas.gameObject.SetActive(true);
            barParent.SetActive(false);
        }
    }
    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("trigger pressed");
        if (gameStarted == false)
        {
            previousBar = Instantiate(barPrefabs[0], spawnPoint.transform.position, Quaternion.identity);
            previousBar.transform.parent = barParent.transform;
            for (int i = 0; i < barLength; i++)
            {

                switch (previousNumber)
                {
                    case 0:
                        currentNumber = Random.Range(0, 3);
                        /*if (numberOfTimes[0] >= 2 && currentNumber == 0)
                        {
                            currentNumber = Random.Range(0, 2);
                            if (currentNumber == 0)
                            {
                                currentNumber = 1;
                            }else if (currentNumber == 1)
                            {
                                currentNumber = 2;
                            }
                            numberOfTimes[0] = 0;
                        }*/
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
            gameStarted = true;
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
}
