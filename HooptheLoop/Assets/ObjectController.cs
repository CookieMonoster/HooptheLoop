using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectController : MonoBehaviour
{
    public GameObject[] scene1Objects;
    public GameObject[] scene2Objects;
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "lobby")
        {
            for (int i = 0; i < scene1Objects.Length; i++)
            {
                scene1Objects[i].SetActive(true);
            }
            for (int i = 0; i < scene2Objects.Length; i++)
            {
                scene2Objects[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < scene1Objects.Length; i++)
            {
                scene1Objects[i].SetActive(false);
            }
            for (int i = 0; i < scene2Objects.Length; i++)
            {
                scene2Objects[i].SetActive(true);
            }
        }
    }
}
