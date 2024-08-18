using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CellItem
{
    public Sprite icon;
    private int m_id;

    public int Id { get => m_id; set => m_id = value; }
}

public enum GameState
{
    Menu,
    Playing,
    GameOver,
    Finish
}

public enum AnimState
{ 
    Idle,
    Flip,
    Explored
}
