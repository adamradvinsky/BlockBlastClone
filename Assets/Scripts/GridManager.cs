using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

using System.Collections;
public class GridManager : MonoBehaviour
{
    public int width = 8;
    public int height = 8;

    public bool lose = false;
    public GameObject tilePrefab;

    public int[,] grid;
    public int[,] highlightGrid;
    public TileScript[,] tiles;

    Vector2Int prevHover = new Vector2Int(-1, -1);
    Vector2Int[] activeShape;

    private Vector2 startPos;
    public float tileScale;
    private Vector2 gridOrigin;

    public GameManager gameMan;

    private List<int> rowClear;
    private List<int> colClear;
    public Animator bruh;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            gameMan.StartCoroutine(gameMan.Lose());
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            bruh.SetBool("lost", false);
        }
    }


    void Awake()
    {

        startPos = transform.position;
        grid = new int[width, height];

        highlightGrid = new int[width, height];

        tiles = new TileScript[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject t = Instantiate(tilePrefab, transform);
                Vector2 pos = startPos + new Vector2(x * tileScale, y * tileScale);
                t.transform.position = pos;

                if (x == 0 && y == 0)
                    gridOrigin = pos;

                TileScript tile = t.GetComponent<TileScript>();
                tile.Init(x, y);

                tiles[x, y] = tile;
                grid[x, y] = 0;
                highlightGrid[x, y] = 0;

            }
        }
    }



    // Called while dragging
    public void Hover(Vector2Int gridPos, Vector2Int[] shape, Color color)
    {

        if (gridPos == prevHover)
            return;

        ClearHoverRC(rowClear, colClear);
        ClearHover(prevHover, shape);

        rowClear = new List<int> { };
        colClear = new List<int> { };

        activeShape = shape;
        bool canPlace = CanPlace(gridPos, shape);


        if (canPlace)
        {
            foreach (Vector2Int offset in shape)
            {
                Vector2Int p = gridPos + offset;
                highlightGrid[p.x, p.y] = 1;
            }


            // check if can clear 
            // then add those to what to highlight


            foreach (var pos in shape)
            {
                Vector2Int p = gridPos + pos;

                if (checkCollumn(p.x, highlightGrid))
                {
                    rowClear.Add(p.x);
                }

                if (checkRow(p.y, highlightGrid))
                {
                    colClear.Add(p.y);
                }
            }
        }
        else
        {
            return;
        }

        highlight(gridPos, shape, color);
        highlightRC(gridPos, rowClear, colClear, Color.red);

        prevHover = gridPos;
    }



    private void highlight(Vector2Int gridPos, IEnumerable<Vector2Int> shape, Color color)
    {
        foreach (Vector2Int offset in shape)
        {
            Vector2Int p = gridPos + offset;

            if (!InBounds(p)) continue;
            if (grid[p.x, p.y] != 0) continue;

            tiles[p.x, p.y].setToHover(color);
        }
    }



    private void highlightRC(Vector2Int gridPos, List<int> row, List<int> col, Color color)
    {
        foreach (int a in row)
        {
            for (int i = 0; i < 8; i++)
            {
                tiles[a, i].setToClearHover();
            }
        }

        foreach (int a in col)
        {
            for (int i = 0; i < 8; i++)
            {
                tiles[i, a].setToClearHover();
            }
        }
    }


    // Called on mouse release
    public bool Place(Vector2Int gridPos, Color color)
    {
        if (!CanPlace(gridPos, activeShape))
            return false;

        foreach (Vector2Int offset in activeShape)
        {
            Vector2Int p = gridPos + offset;
            if (!InBounds(p)) continue;

            grid[p.x, p.y] = 1;
            highlightGrid[p.x, p.y] = 1;
            tiles[p.x, p.y].setToFill(color);
            tiles[p.x, p.y].setToNotHover();
        }

        ClearHover(prevHover, activeShape);
        // ADD SCORE
        gameMan.addScore(30);
        gameMan.shapeCountDecrease();



        checkAClear();

        return true;
    }


    public void checkForLoss()
    {
        List<GameObject> list = gameMan.getInGameBlocks();
        foreach (var a in list)
        {
            Vector2Int[] b = a.GetComponent<shape>().finalBlock;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (CanPlace(new Vector2Int(i, j), b))
                    {
                        //Debug.Log("can place at " + i +  " : " + j + " using " + a.name);
                        return;
                    }
                }
            }
        }



        lose = true;
    }




    private void checkAClear()
    {
        List<int> rows = new List<int>();
        List<int> colls = new List<int>();


        // check row
        for (int i = 0; i < 8; i++)
        {
            if (checkRow(i, grid))
            {
                rows.Add(i);
                Debug.Log("row " + i + " is a clear");
            }
        }


        for (int i = 0; i < 8; i++)
        {
            if (checkCollumn(i, grid))
            {
                colls.Add(i);
                Debug.Log("collumn " + i + " is a clear");
            }
        }
        StartCoroutine(ClearColRow(rows, colls));
    }


    private bool checkRow(int a, int[,] grid)
    {

        for (int i = 0; i < 8; i++)
        {
            if (grid[i, a] == 0)
            {
                return false;
            }
        }
        return true;
    }


    private bool checkCollumn(int a, int[,] grid)
    {
        for (int i = 0; i < 8; i++)
        {
            if (grid[a, i] == 0)
            {
                return false;
            }
        }

        return true;
    }

    public IEnumerator ClearColRow(List<int> rows, List<int> colls)
    {

        yield return new WaitForSeconds((float)0.2);
        foreach (int a in rows)
        {
            for (int i = 0; i < 8; i++)
            {
                grid[i, a] = 0;
                highlightGrid[i, a] = 0;
                tiles[i, a].settoClear();
                tiles[i, a].setToEmpty();
                tiles[i, a].setToNotHover();
                tiles[i, a].setToNotClearHover();
            }
        }

        foreach (int a in colls)
        {
            for (int i = 0; i < 8; i++)
            {
                grid[a, i] = 0;
                highlightGrid[a, i] = 0;
                tiles[a, i].setToEmpty();
                tiles[a, i].setToNotHover();
                tiles[a, i].setToNotClearHover();
            }
        }

        yield return null;
    }



    void ClearHover(Vector2Int gridPos, Vector2Int[] shape)
    {
        if (shape == null) return;

        foreach (Vector2Int offset in shape)
        {
            Vector2Int p = gridPos + offset;
            if (!InBounds(p)) continue;

            if (grid[p.x, p.y] == 0)
            {
                highlightGrid[p.x, p.y] = 0;
                tiles[p.x, p.y].setToNotHover();
            }
        }
    }

    void ClearHoverRC(List<int> row, List<int> col)
    {
        if (row == null || col == null)
        {
            return;
        }

        foreach (int a in row)
        {
            for (int i = 0; i < 8; i++)
            {
                if (highlightGrid[a, i] == 1)
                {
                    tiles[a, i].setToNotHover();
                    tiles[a, i].setToNotClearHover();
                }
            }
        }

        foreach (int a in col)
        {

            for (int i = 0; i < 8; i++)
            {
                if (highlightGrid[i, a] == 1)
                {
                    tiles[i, a].setToNotClearHover();
                }
            }
        }
    }

    public bool CanPlace(Vector2Int gridPos, Vector2Int[] shape)
    {
        foreach (Vector2Int offset in shape)
        {
            Vector2Int p = gridPos + offset;

            if (!InBounds(p)) return false;
            if (grid[p.x, p.y] != 0) return false;
        }
        return true;
    }

    bool InBounds(Vector2Int p)
    {
        return p.x >= 0 && p.y >= 0 && p.x < width && p.y < height;
    }
}