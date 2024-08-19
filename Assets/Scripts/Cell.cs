using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    private int m_id;
    private bool m_isShow;
    private Animator m_anim;

    [SerializeField] private Sprite frontBG;
    [SerializeField] private Sprite backBG;
    [SerializeField] private Image iconBG;
    [SerializeField] private Image icon;
    [SerializeField]  private Button button;
    

    public int Id { get => m_id; set => m_id = value; }

    private void Awake()
    {
        m_anim = GetComponent<Animator>();
    }

    public void SetIcon(Sprite _icon)
    {
        if (iconBG)
            iconBG.sprite = backBG;

        if (icon)
            icon.sprite = _icon;
    }
}
