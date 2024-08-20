using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instace { get; private set; }

    [SerializeField] private CellItem[] cellItems;
    [SerializeField] private Cell cellPref;
    [SerializeField] private Transform grid;

    /*static variable doesn't get destroyed on sceneloading*/
    public static int g_row;
    public static int g_col;
    public static float g_timeLimit;
    
    public GameState gameState;
    public int score;
    public int bestScore;
    private List<Cell> m_cellSelected;
    private int m_totalCellNum;
    private int m_currentCorrectPair;
    private List<CellItem> m_totalCell;
    private float m_timeCount;
    private float m_timeLimit;

    private void Awake()
    {
        if (Instace != null && Instace != this)
            Destroy(this);
        else
            Instace = this;

        m_cellSelected = new List<Cell>();
        m_totalCellNum = 0;
        m_currentCorrectPair = 0;
        m_totalCell = new List<CellItem>();

    }

    public void Start()
    {
    }

    public void Update()
    {
        if (gameState != GameState.Playing) return;

        m_timeCount -= Time.deltaTime;

        if (m_timeCount <= 0 && gameState != GameState.GameOver)
        {
            gameState = GameState.GameOver;
            m_timeCount = 0;
            if (ScreenManager.Instace)
                ScreenManager.Instace.GameoverDialog.Show(true);
        }

        if (ScreenManager.Instace)
            ScreenManager.Instace.UpdateTimeProgress((float)m_timeCount, (float)m_timeLimit);
    }

    public void GenerateLevel(int row, int col, float _timeLimit)
    {
        // this doesn't get destroyed on Load
        g_row = row; g_col = col; g_timeLimit = _timeLimit;

        gameState = GameState.Playing;
        m_timeCount = _timeLimit;
        m_timeLimit = _timeLimit;
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
                    
                    //sound sfx flip
                    if (AudioManager.Instace)
                        AudioManager.Instace.PlaySFX(AudioManager.Instace.flip);

                    //TODO: run anim reveal
                    cellClone.runRevealAnim();
                    if (m_cellSelected.Count >= 2 && m_cellSelected.Count % 2 == 0)
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
        yield return new WaitForSeconds(1f);

        bool isMatch = m_cellSelected[0] != null && m_cellSelected[1] != null && m_cellSelected[0].Id == m_cellSelected[1].Id;

        if (isMatch)
        {
            m_currentCorrectPair++;
            // check only the first 2 selected
            for (int i = 0;i < 2;i++)
            {
                Cell cell = m_cellSelected[i];
                if (cell != null)
                {
                    //sound sfx right match
                    if (AudioManager.Instace)
                        AudioManager.Instace.PlaySFX(AudioManager.Instace.right);
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
                    //sound sfx flip 
                    if (AudioManager.Instace)
                    {
                        AudioManager.Instace.PlaySFX(AudioManager.Instace.flip);
                        AudioManager.Instace.PlaySFX(AudioManager.Instace.wrong);
                    }
                    //TODO: run anim FLIP back
                    cell.runRevealAnim();
                    cell.button.enabled = true;
                }
            }
        }

        m_cellSelected.RemoveAt(0);
        m_cellSelected.RemoveAt(0);

        // check game win
        if (m_currentCorrectPair*2 == m_totalCellNum)
        {
            // scoring
            score = (int)Mathf.Ceil(m_timeCount);
            if (score > bestScore)
                bestScore = score;

            gameState = GameState.LevelComplete;
            if (ScreenManager.Instace)
                ScreenManager.Instace.LevelCompleteDialog.Show(true);
        }
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("bestScore", bestScore);
    }
    private void OnEnable()
    {
        bestScore = PlayerPrefs.GetInt("bestScore");
    }
}
