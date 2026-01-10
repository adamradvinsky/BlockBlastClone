using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GridManager : MonoBehaviour
{
    public int width = 8;
    public int height = 8;

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
    public void Hover(Vector2Int gridPos, Vector2Int[] shape)
    {

        if (gridPos == prevHover)
            return;

        ClearHover(prevHover, shape);

        activeShape = shape;
        bool canPlace = CanPlace(gridPos, shape);
        Color color;

        List<Vector2Int> clearShape = new List<Vector2Int>();



        if (canPlace)
        {
            foreach (Vector2Int offset in shape)
            {
                Vector2Int p = gridPos + offset;
                highlightGrid[p.x, p.y] = 1;
            }

            color = Color.green;
            // check if can clear 
            // then add those to what to highlight


            foreach (var pos in shape)
            {
                Vector2Int p = gridPos + pos;
                //Debug.Log("x: " + p.x + " y: " + p.y);
                if (checkCollumn(p.x, highlightGrid))
                {
                    Debug.Log("ALERT collumn");

                    // // add collumn to highlight
                    for (int i = 0; i < 8; i++)
                    {
                        clearShape.Add(new Vector2Int(p.x, i));
                    }

                }

                if (checkRow(p.y, highlightGrid))
                {
                    Debug.Log("ALERT ROW");

                    // add row to highlight
                    for (int i = 0; i < 8; i++)
                    {
                        clearShape.Add(new Vector2Int(i, p.y));
                    }
                }
            }
        }
        else
        {
            return;
        }

        highlight(gridPos, shape, color);
        highlightRC(gridPos, clearShape, Color.red);

        prevHover = gridPos;
    }



    private void highlight(Vector2Int gridPos, IEnumerable<Vector2Int> shape, Color color)
    {
        foreach (Vector2Int offset in shape)
        {
            Vector2Int p = gridPos + offset;

            if (!InBounds(p)) continue;
            if (grid[p.x, p.y] != 0) continue;

            tiles[p.x, p.y].SetColor(color);
        }
    }

    private void highlightRC(Vector2Int gridPos, IEnumerable<Vector2Int> shape, Color color)
    {
        foreach (Vector2Int offset in shape)
        {
            Vector2Int p = gridPos + offset;


            tiles[offset.x, offset.y].SetColor(color);
        }
    }


    // Called on mouse release
    public bool Place(Vector2Int gridPos)
    {
        if (!CanPlace(gridPos, activeShape))
            return false;

        foreach (Vector2Int offset in activeShape)
        {
            Vector2Int p = gridPos + offset;
            if (!InBounds(p)) continue;

            grid[p.x, p.y] = 1;
            highlightGrid[p.x, p.y] = 1;
            tiles[p.x, p.y].SetColor(Color.blue);
        }


        // ADD SCORE
        gameMan.addScore(30);
        gameMan.shapeCountDecrease();

        checkAClear();

        return true;
    }



    private void extraHover()
    {

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
        clearColRow(rows, colls);
    }

    private bool checkRow(int a, int[,] grid)
    {

        for (int i = 0; i < 8; i++)
        {
            if (grid[i, a] == 0)
            {

                //Debug.Log("x: " + i + " y: " + a + " bad boy");
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

    private void clearColRow(List<int> rows, List<int> colls)
    {
        foreach (int a in rows)
        {
            for (int i = 0; i < 8; i++)
            {
                grid[i, a] = 0;
                highlightGrid[i, a] = 0;
                tiles[i, a].SetColor(Color.white);
            }
        }

        foreach (int a in colls)
        {
            for (int i = 0; i < 8; i++)
            {
                grid[a, i] = 0;
                highlightGrid[a, i] = 0;
                tiles[a, i].SetColor(Color.white);
            }
        }
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
                tiles[p.x, p.y].SetColor(Color.white);
            }
        }
    }

    void ClearHoverRC(Vector2Int gridPos, Vector2Int[] shape)
    {
        if (shape == null) return;

        foreach (Vector2Int offset in shape)
        {
            Vector2Int p = gridPos + offset;
            if (!InBounds(p)) continue;

            if (grid[p.x, p.y] == 0)
            {
                highlightGrid[p.x, p.y] = 0;
                tiles[p.x, p.y].SetColor(Color.white);
            }
        }
    }

    bool CanPlace(Vector2Int gridPos, Vector2Int[] shape)
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