using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreAdder : MonoBehaviour
{
    public ParticleSystem explosion;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "ScoreCollider")
        {
            GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().addScore(100);
            explosion.Play();
            Destroy(this.gameObject);
        }

    }
}
