using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;


public class CollisionTester : MonoBehaviour
{
    public GameManager _gameManager;
    private void Start()
    {
        
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _gameManager.gameOver = true;
            
            Destroy(this.gameObject);
        }

    }
}
