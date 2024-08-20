using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instace { get; private set; }

    [SerializeField] private Button button2x2;
    [SerializeField] private Button button3x4;
    [SerializeField] private Button button5x6;
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private GameObject gameplayScreen;
    [SerializeField] private Image timeBar;
    [SerializeField] public PauseDialog PauseDialog;
    [SerializeField] public GameOverDialog GameoverDialog;
    [SerializeField] public LevelCompleteDialog LevelCompleteDialog;

    public void Awake()
    {
        if (Instace != null && Instace != this)
            Destroy(this);
        else
            Instace = this;

        button2x2.onClick.RemoveAllListeners();
        button2x2.onClick.AddListener(() =>
        {
            changeScreen(true);
            GameManager.Instace.GenerateLevel(2, 2, 10);
        });
        button3x4.onClick.RemoveAllListeners();
        button3x4.onClick.AddListener(() =>
        {
            changeScreen(true);
            GameManager.Instace.GenerateLevel(3, 4, 60);
        });
        button5x6.onClick.RemoveAllListeners();
        button5x6.onClick.AddListener(() =>
        {
            changeScreen(true);
            GameManager.Instace.GenerateLevel(5, 6, 120);
        });
    }

    public void changeScreen(bool isGameplayScreen)
    {
        if (gameplayScreen)
            gameplayScreen.SetActive(isGameplayScreen);

        if (mainMenuScreen)
            mainMenuScreen.SetActive(!isGameplayScreen);
    }

    public void UpdateTimeProgress(float curr, float total)
    {
        float rate = curr / total;
        if (timeBar)
            timeBar.fillAmount = rate;
    }
}
