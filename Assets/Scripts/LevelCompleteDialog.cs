using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelCompleteDialog : MonoBehaviour
{
    [SerializeField] private Text score;
    [SerializeField] private Text bestScore;
    public void Show(bool isShow)
    {
        gameObject.SetActive(isShow);

        if (score && GameManager.Instace)
            score.text = GameManager.Instace.score.ToString();
        if (bestScore && GameManager.Instace)
            bestScore.text = GameManager.Instace.bestScore.ToString();
    }

    public void Continue()
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
