using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        gameObject.SetActive(isCloseDialog);
    }
}
