using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarTranslate : MonoBehaviour
{
   public float translateFactor = 1f;
    public GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }
    void FixedUpdate()
    {
        this.transform.Translate(Vector3.left * (translateFactor / 10f));
    }
}
