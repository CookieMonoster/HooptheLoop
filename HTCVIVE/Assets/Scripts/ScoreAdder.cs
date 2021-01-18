using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreAdder : MonoBehaviour
{
    public LevelManager _levelManager;
    private void Start()
    {

        _levelManager = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();
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
