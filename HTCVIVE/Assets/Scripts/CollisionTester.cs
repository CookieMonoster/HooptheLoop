using UnityEngine;


public class CollisionTester : MonoBehaviour
{
    public bool doesSetState;

    public bool isCollided;
    public bool toDestroy;
    public string tagName;

    

    private void OnTriggerEnter(Collider other)
    {
        LevelManager levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();


        if (other.tag == tagName)
        {
            if(doesSetState)
            {
                levelManager.setGameState(1, true);
                levelManager.playLoseSound();
                
            }
            isCollided = true;
            if (toDestroy)
            {
                
                Destroy(this.gameObject);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == tagName)
        {
            isCollided = false;
        }
    }
}
