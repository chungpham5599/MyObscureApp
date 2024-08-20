using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverDialog : MonoBehaviour
{
    public void Show(bool isShow)
    {
        //sound sfx flip
        if (AudioManager.Instace)
            AudioManager.Instace.PlaySFX(AudioManager.Instace.gameover);
        gameObject.SetActive(isShow);
    }
    public void BackToMainMenu()
    {
        if (SceneManager.GetActiveScene() != null)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Replay()
    {
        SceneManager.sceneLoaded += OnSceneLoadedEvent;
        if (SceneManager.GetActiveScene() != null)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    private void OnSceneLoadedEvent(Scene scene, LoadSceneMode mode)
    {
        if (ScreenManager.Instace)
            ScreenManager.Instace.changeScreen(true);
        if (GameManager.Instace)
            GameManager.Instace.GenerateLevel(GameManager.g_row,
                                            GameManager.g_col,
                                            GameManager.g_timeLimit);

        SceneManager.sceneLoaded -= OnSceneLoadedEvent;
    }    
}
