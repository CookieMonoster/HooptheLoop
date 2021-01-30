using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;


public class CollisionTester : MonoBehaviour
{
    public bool doesSetState;

    public bool isCollided;
    public bool toDestroy;
    public string tagName;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == tagName)
        {
            if(doesSetState)
            {
                GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>().setGameState(1, true);
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
