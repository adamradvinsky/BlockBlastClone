using System;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class shape : MonoBehaviour
{
    public GridManager grid;

    public float tileSize;
    public Vector2 gridOrigin;


    public Vector2Int[] block = new Vector2Int[]
    {
        // collumn : row
        new Vector2Int(0, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, 2),
        new Vector2Int(1, 2),
        new Vector2Int(2, 2),
    };

    public Vector2 startPos;



    void Start()
    {
        tileSize = 1f;
        gridOrigin = grid.tiles[0,0].transform.position;
    }

    void OnMouseDrag()
    {
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mouse.x, mouse.y, -5);

        Vector2Int gridPos = WorldToGrid(mouse);
        grid.Hover(gridPos, block);
    }

    void OnMouseUp()
    {
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int gridPos = WorldToGrid(mouse);

        grid.Place(gridPos);
        transform.position = startPos;
    }

    public Vector2Int WorldToGrid(Vector2 world)
    {
        Vector2 local = world - gridOrigin;

        int x = Mathf.RoundToInt(local.x / tileSize);
        int y = Mathf.RoundToInt(-local.y / tileSize);

        return new Vector2Int(x, y);
    }

}
