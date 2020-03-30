using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionTester : MonoBehaviour
{
    public gameManager _gameManager;

    private void Start()
    {
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<gameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            _gameManager.gameOver = true;
            Destroy(this.gameObject);
        }
 
    }
}
