using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barTranslate : MonoBehaviour
{
    public float translateFactor = 1f;
    public gameManager _gameManager;

    private void Start()
    {
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<gameManager>();
    }
    void FixedUpdate()
    {
        if(_gameManager.gameStarted == true)
        {
            this.transform.Translate(Vector3.left * (translateFactor / 10f));
        }
    }
}
