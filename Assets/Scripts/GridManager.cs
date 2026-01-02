using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GridManager : MonoBehaviour
{
    public int width = 8;
    public int height = 8;

    public GameObject tilePrefab;

    public int[,] grid;
    public TileScript[,] tiles;

    Vector2Int prevHover = new Vector2Int(-1, -1);
    Vector2Int[] activeShape;

    public Vector2 startPos;
    public float tilex;
    public float tiley;
    public Vector2 gridOrigin;


    private int score = 0;
    public int clearPoints = 100;

    void Start()
    {

        startPos = transform.position;
        grid = new int[width, height];
        tiles = new TileScript[width, height];
        tilex = tilePrefab.transform.localScale.x;
        tiley = tilePrefab.transform.localScale.y;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject t = Instantiate(tilePrefab, transform);
                Vector2 pos = startPos + new Vector2(x * tilex, -y * tiley);
                t.transform.position = pos;

                if (x == 0 && y == 0)
                    gridOrigin = pos;

                TileScript tile = t.GetComponent<TileScript>();
                tile.Init(x, y);

                tiles[x, y] = tile;
                grid[x, y] = 0;

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

        if (canPlace)
        {
            color = Color.green;
        }
        else
        {
            return;
        }

        foreach (Vector2Int offset in shape)
        {
            Vector2Int p = gridPos + offset;

            if (!InBounds(p)) continue;
            if (grid[p.x, p.y] != 0) continue;

            tiles[p.x, p.y].SetColor(color);


            if (checkRow(p.y))
            {

            }

            if (checkCollumn(p.x))
            {

            }
        }



        prevHover = gridPos;
    }

    // Called on mouse release
    public void Place(Vector2Int gridPos)
    {
        if (!CanPlace(gridPos, activeShape))
            return;

        foreach (Vector2Int offset in activeShape)
        {
            Vector2Int p = gridPos + offset;
            if (!InBounds(p)) continue;

            grid[p.x, p.y] = 1;
            tiles[p.x, p.y].SetColor(Color.blue);
        }

        Debug.Log("SCORE: " + score);

        if (checkAClear() > 0)
        {
            Debug.Log(" A CLEAR");
        }
    }


    private int checkAClear()
    {
        int clear = 0;

        // check row
        for (int i = 0; i < 8; i++)
        {
            if (checkRow(i))
            {
                clear++;
            }
        }


        for (int i = 0; i < 8; i++)
        {
            if (checkCollumn(i))
            {
                clear++;
            }
        }

        return clear;

    }

    private bool checkRow(int a)
    {
        for (int i = 0; i < 8; i++)
        {
            if (grid[a, i] == 0)
            {
                return false;
            }
        }

        // remove this row
        score += clearPoints;

        setRow(Color.white, 0, a);

        return true;
    }


    private bool checkCollumn(int a)
    {

        for (int i = 0; i < 8; i++)
        {
            if (grid[i, a] == 0)
            {
                return false;
            }
        }

        // remove this collummn
        score += clearPoints;

        setCollumn(Color.white, 0, a);

        return true;
    }

    private void setRow(Color color, int a, int index)
    {
        for (int i = 0; i < 8; i++)
        {
            grid[index, i] = a;
            tiles[index, i].SetColor(color);
        }
    }

    private void setCollumn(Color color, int a, int index)
    {
        for (int i = 0; i < 8; i++)
        {
            grid[i, index] = a;
            tiles[i, index].SetColor(Color.white);
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
                tiles[p.x, p.y].SetColor(Color.white);
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