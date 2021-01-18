using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;


public class CollisionTester : MonoBehaviour
{
    public LevelManager _levelManager;
    private void Start()
    {

        _levelManager = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _levelManager.setGameState(1, true);


            Destroy(this.gameObject);
        }

    }
}
