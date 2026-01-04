using System;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class shape : MonoBehaviour
{
    public GridManager grid;

    private float tileSize;
    private Vector2 gridOrigin;
    public GameObject prefabBlock;

    private Vector2 snapPos;


    public Vector2Int[] block = new Vector2Int[]
    {
        // collumn : row
        new Vector2Int(0, 0)
    };




    void Start()
    {
        tileSize = grid.tileScale;
        snapPos = transform.position;
        gridOrigin = grid.tiles[0, 0].transform.position;

        BoxCollider2D boxCol = GetComponent<BoxCollider2D>();

        int xR = 0;
        int xL = 0;

        int yR = 0;
        int yL = 0;

        // set up the blocks
        foreach (var pos in block)
        {
            GameObject t = Instantiate(prefabBlock, transform);
            Vector3 newPos = transform.position + new Vector3(pos.x * tileSize, pos.y * tileSize, transform.position.y);
            t.transform.position = newPos;

            int a = pos.y == 0 ? 0 : pos.y > 0 ? 1 : -1;
            int b = pos.x == 0 ? 0 : pos.x > 0 ? 1 : -1;


            xR = pos.x > xR ? pos.x : xR;
            xL = pos.x < xL ? pos.x : xL;

            yR = pos.y > yR ? pos.y : yR;
            yL = pos.y < yL ? pos.y : yL;


        }
        int bruh = (yR + yL);
        Debug.Log("ylyr" + bruh);
        boxCol.size = new Vector2(boxCol.size.x + (Mathf.Abs(xR) + Mathf.Abs(xL)) * tileSize, boxCol.size.y + (Mathf.Abs(yR) + Mathf.Abs(yL)) * tileSize);
        boxCol.offset = new Vector2(boxCol.offset.x + (xR + xL) * tileSize / 2, boxCol.offset.y + (yR + yL) * tileSize / 2);
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

        if (grid.Place(gridPos))
        {
            Destroy(this.gameObject);
        }
        else
        {
            transform.position = snapPos;
        }
    }

    public Vector2Int WorldToGrid(Vector2 world)
    {
        Vector2 local = world - gridOrigin;

        int x = Mathf.RoundToInt(local.x / tileSize);
        int y = Mathf.RoundToInt(local.y / tileSize);

        return new Vector2Int(x, y);
    }

}
