using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteDialog : MonoBehaviour
{
    public void Show(bool isShow)
    {
        gameObject.SetActive(isShow);
    }
}
