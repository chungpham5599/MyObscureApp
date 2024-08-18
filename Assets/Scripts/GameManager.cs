using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instace { get; private set; }

    [SerializeField]
    public CellItem[] cellItems;
    public GameObject cellPref;
    public Transform grid;
    [SerializeField]

    public GameState gameState;
    private List<GameObject> m_cellPref1;
    private List<GameObject> m_cellPref2;
    private int m_totalCellNum;
    private List<CellItem> m_totalCell;

    private void Awake()
    {
        if (Instace != null && Instace != this)
            Destroy(this);
        else
            Instace = this;

        m_cellPref1 = new List<GameObject>();
        m_cellPref2 = new List<GameObject>();
        m_totalCell = new List<CellItem>();

        gameState = GameState.Playing;
    }

    public void Start()
    {
        GenerateGrid(2, 2);
    }

    public void GenerateGrid(int width, int height)
    {
        m_totalCellNum = width * height;

        for (int i = 0; i < m_totalCellNum / 2; i++)
        {
            //pick one random icon
            var random = Random.Range(0, cellItems.Length);
            m_totalCell.Add(cellItems[random]); 
            m_totalCell.Add(cellItems[random]);
        }

        ShuffleCells();
        ClearGrid();

        for (int i = 0; i < m_totalCell.Count; i++)
        {
            var cell = m_totalCell[i];

            var cellClone = Instantiate(cellPref, Vector3.zero, Quaternion.identity);
            cellClone.transform.SetParent(grid);
            cellClone.transform.localPosition = Vector3.zero;
            cellClone.transform.localScale = Vector3.one;
        }
    }

    private void ClearGrid()
    {
        if (!grid) return;

        for (int i = 0; i < grid.childCount; i++)
        {
            var child = grid.GetChild(i);
            if (child)
                Destroy(child.gameObject);
        }
    }

    private void ShuffleCells()
    {
        if (m_totalCell.Count <= 0 || m_totalCell == null || m_totalCellNum <= 0) return;

        for (int i = 0; i < m_totalCell.Count; i++)
        {
            var temp = m_totalCell[i];
            if (temp != null)
            {
                int randomID = Random.Range(0, m_totalCell.Count);
                m_totalCell[i] = m_totalCell[randomID];
                m_totalCell[randomID] = temp;
            }
        }
    }
}
