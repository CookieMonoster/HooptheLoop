using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitioner : MonoBehaviour
{
    public Animator animator;

    private int levelToLoad;
    
    public void FadeToNextLevel()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void FadeToLevel (int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }
    public void FadeIn()
    {
        animator.Play("FadeIn");
    }
    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
        animator.Play("FadeIn");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }
}
