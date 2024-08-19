using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    private int m_id;
    private bool m_isRevealed;
    private Animator m_anim;

    [SerializeField] private Sprite frontBG;
    [SerializeField] private Sprite backBG;
    [SerializeField] private Image iconBG;
    [SerializeField] private Image icon;
    [SerializeField] public Button button;
    

    public int Id { get => m_id; set => m_id = value; }

    private void Awake()
    {
        m_anim = GetComponent<Animator>();
        m_isRevealed = false;
    }

    public void SetIcon(Sprite _icon)
    {
        if (iconBG)
            iconBG.sprite = backBG;

        if (icon)
        {
            icon.sprite = _icon;
            icon.gameObject.SetActive(false);
        }    
    }

    public void ChangeState()
    {
        m_isRevealed = !m_isRevealed;

        if (iconBG)
            iconBG.sprite = m_isRevealed ? frontBG : backBG;

        if (icon)
            icon.gameObject.SetActive(m_isRevealed);
    }

    public void runRevealAnim()
    {
        if (m_anim)
            m_anim.SetBool(AnimState.Flip.ToString(), true);
    }
    public void runExploreAnim()
    {
        if (m_anim)
            m_anim.SetBool(AnimState.Explored.ToString(), true);
    }
    public void runIdleAnim()
    {
        if (m_anim)
            m_anim.SetBool(AnimState.Flip.ToString(), false);
    }
}
