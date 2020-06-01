using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    public GameObject m_sceneTransitioner;
    public void transitionScene()
    {
        m_sceneTransitioner.SetActive(true);
    }
}

