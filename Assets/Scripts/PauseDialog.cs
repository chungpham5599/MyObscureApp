using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseDialog : MonoBehaviour
{
    public void StartPausing(bool isShowDialog)
    {
        gameObject.SetActive(isShowDialog);

        Time.timeScale = 0f;
    }

    public void Resume(bool isCloseDialog)
    {
        Time.timeScale = 1f;
        gameObject.SetActive(!isCloseDialog);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        if (SceneManager.GetActiveScene() != null)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }    
}
