using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class spawnPoint : MonoBehaviour
{
    public SteamVR_Action_Boolean spawnTube;
    public SteamVR_Input_Sources handType;
    public GameObject tube;
    public gameManager _gameManager;

    

    


    private void Start()
    {
        spawnTube.AddOnStateDownListener(TriggerDown, handType);
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<gameManager>();
        //spawnTube.AddOnStateUpListener(TriggerUp, handType);
    }
   // public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
   //{
   //     Debug.Log("Trigger is up");
   //     
   // }
    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Trigger is down");
        if (_gameManager.gameStarted == false)
        {
            
            Instantiate(tube, transform.position, Quaternion.identity);
            _gameManager.gameStarted = true;
        }
        
        
    }
}
