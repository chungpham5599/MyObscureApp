using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instace { get; private set; }

    [SerializeField]
    public CellItem[] cellItems;
    public Cell cellPref;
    public Transform grid;
    [SerializeField]

    public GameState gameState;
    private List<Cell> m_cellSelected;
    private int m_totalCellNum;
    private int m_currentCorrectPair;
    private List<CellItem> m_totalCell;

    private void Awake()
    {
        if (Instace != null && Instace != this)
            Destroy(this);
        else
            Instace = this;

        gameState = GameState.Playing;
        m_cellSelected = new List<Cell>();
        m_totalCellNum = 0;
        m_currentCorrectPair = 0;
        m_totalCell = new List<CellItem>();

    }

    public void Start()
    {
        GenerateGrid(3, 4);
    }

    public void GenerateGrid(int row, int col)
    {
        m_totalCellNum = row * col;

        for (int i = 0; i < m_totalCellNum / 2; i++)
        {
            //pick one random icon
            int random = Random.Range(0, cellItems.Length);
            m_totalCell.Add(cellItems[random]); 
            m_totalCell.Add(cellItems[random]);
            cellItems[random].Id = random;
        }

        ShuffleCells();
        ClearGrid();

        for (int i = 0; i < m_totalCell.Count; i++)
        {
            CellItem cell = m_totalCell[i];

            var cellClone = Instantiate(cellPref, Vector3.zero, Quaternion.identity);
            cellClone.transform.SetParent(grid);
            cellClone.transform.localPosition = Vector3.zero;
            cellClone.transform.localScale = Vector3.one;

            // update icon arcording to its ID
            cellClone.SetIcon(cell.icon);
            cellClone.Id = cell.Id;

            // bind event handler
            if (cellClone.button)
            {
                cellClone.button.onClick.RemoveAllListeners();
                cellClone.button.onClick.AddListener(() =>
                {
                    m_cellSelected.Add(cellClone);
                    //TODO: run anim reveal
                    cellClone.runRevealAnim();
                    if (m_cellSelected.Count >= 2)
                    {
                        StartCoroutine(CheckPairCo());
                    }
                    cellClone.button.enabled = false;
                });
            }
        }
        Vector2 containerSize = grid.GetComponent<RectTransform>().rect.size;
        Vector2 spacing = new Vector2(20,20);
        Vector2 cellSize = new Vector2(containerSize.x / col - spacing.x, containerSize.y / row - spacing.y);
        grid.GetComponent<GridLayoutGroup>().cellSize = cellSize;
        grid.GetComponent<GridLayoutGroup>().spacing = spacing;
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

    private IEnumerator CheckPairCo()
    {
        bool isMatch = m_cellSelected[0] != null && m_cellSelected[1] != null && m_cellSelected[0].Id == m_cellSelected[1].Id;

        yield return new WaitForSeconds(1f);

        if (m_cellSelected != null && m_cellSelected.Count >= 2)
        {
            if (isMatch)
            {
                m_currentCorrectPair++;
                // check only the first 2 selected
                for (int i = 0;i < 2;i++)
                {
                    Cell cell = m_cellSelected[i];
                    if (cell != null)
                    {
                        //TODO: run anim explore
                        cell.runExploreAnim();
                    }
                }
            }
            else
            {
                // check only the first 2 selected
                for (int i = 0; i < 2; i++)
                {
                    Cell cell = m_cellSelected[i];
                    if (cell != null)
                    {
                        //TODO: run anim FLIP back
                        cell.runRevealAnim();
                        cell.button.enabled = true;
                    }
                }
            }

            m_cellSelected.RemoveAt(0);
            m_cellSelected.RemoveAt(0);
        }

        // check game win
        if (m_currentCorrectPair*2 == m_totalCellNum)
        {
            Debug.Log("Game Win!");
        }
    }
}
