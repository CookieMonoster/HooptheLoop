using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreAdder : MonoBehaviour
{
    public GameManager _gameManager;
    private void Start()
    {

        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerScoreTrigger")
        {
            Debug.Log("Score + 1");
            Destroy(this.gameObject);
        }

    }
}
