using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarTranslate : MonoBehaviour
{
    public float translateFactor = 1f;
    public LevelManager _levelManager;

    private void Start()
    {
        _levelManager = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();
    }
    void FixedUpdate()
    {
        this.transform.Translate(Vector3.left * (translateFactor / 10f));
    }
}
