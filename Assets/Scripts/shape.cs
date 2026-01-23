using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class shape : MonoBehaviour
{
    public GridManager grid;

    private float tileSize;
    private Vector2 gridOrigin;
    public GameObject prefabBlock;


    public GameManager gameMan;
    private Vector2 snapPos;
    private Color color = Color.blue;

    public Vector2Int[] block = new Vector2Int[]
    {
        // collumn : row
        new Vector2Int(0, 0)
    };

    public Vector2Int[] finalBlock;
    private List<Color> colours = new List<Color>
    {
        Color.red,
        Color.blue,
        Color.yellow,
        Color.green,
        Color.pink
    };



    void Awake()
    {
        finalBlock = new Vector2Int[block.Length];

        copyArray(finalBlock, block);
        flip(finalBlock);

        //flip(block);
        int colourNum = UnityEngine.Random.Range(0, colours.Count - 1);
        color = Color.red;
        color = colours[colourNum];

    }

    private void copyArray(Vector2Int[] a, Vector2Int[] b)
    {
        for (int i = 0; i < b.Length; i++)
        {

            a[i] = b[i];
        }
    }


    void Start()
    {


        tileSize = grid.tileScale;
        snapPos = transform.position;
        gridOrigin = grid.tiles[0, 0].transform.position;


        setUp(tileSize);

        //transform.localScale = new Vector3(transform.localScale.x / 2, transform.localScale.y / 2, transform.localScale.z);

    }



    private void setUp(float scale)
    {
        BoxCollider2D badbox = gameObject.GetComponent<BoxCollider2D>();
        Destroy(badbox);

        BoxCollider2D boxCol = gameObject.AddComponent<BoxCollider2D>();

        int xR = 0;
        int xL = 0;

        int yR = 0;
        int yL = 0;

        // set up the blocks
        foreach (var pos in finalBlock)
        {
            GameObject t = Instantiate(prefabBlock, transform);
            setColour(t, color);
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
        boxCol.size = new Vector2(boxCol.size.x + (Mathf.Abs(xR) + Mathf.Abs(xL)) * tileSize, boxCol.size.y + (Mathf.Abs(yR) + Mathf.Abs(yL)) * tileSize);
        boxCol.offset = new Vector2(boxCol.offset.x + (xR + xL) * tileSize / 2, boxCol.offset.y + (yR + yL) * tileSize / 2);

    }

    private void setColour(GameObject t, Color colour)
    {

        t.transform.Find("Square").GetComponent<SpriteRenderer>().color = colour;
    }

    void OnMouseDown()
    {
        //transform.localScale = new Vector3(transform.localScale.x * 2, transform.localScale.y * 2, transform.localScale.z);

    }

    void OnMouseDrag()
    {
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mouse.x, mouse.y, -5);

        Vector2Int gridPos = WorldToGrid(mouse);
        grid.Hover(gridPos, finalBlock, color);
    }

    void OnMouseUp()
    {
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int gridPos = WorldToGrid(mouse);



        if (grid.CanPlace(gridPos, finalBlock))
        {
            grid.Place(gridPos, color);
            gameMan.removeBlockFromGame(this.gameObject);
            grid.checkForLoss();

            Destroy(this.gameObject);
        }
        else
        {
                //transform.localScale = new Vector3(transform.localScale.x / 2, transform.localScale.y / 2, transform.localScale.z);


                transform.position = snapPos;

            if (grid.lose)
            {
                gameMan.StartCoroutine(gameMan.Lose());
            }

        }
    }

    public Vector2Int WorldToGrid(Vector2 world)
    {
        Vector2 local = world - gridOrigin;

        int x = Mathf.RoundToInt(local.x / tileSize);
        int y = Mathf.RoundToInt(local.y / tileSize);

        return new Vector2Int(x, y);
    }

    public void rotate(Vector2Int[] ablock)
    {
        for (int i = 0; i < ablock.Length; i++)
        {
            Vector2Int arotat = new Vector2Int(ablock[i].y, -ablock[i].x);
            this.block[i] = arotat;
        }
    }

    public void flip(Vector2Int[] ablock)
    {
        for (int i = 0; i < block.Length; i++)
        {
            Vector2Int arotat = new Vector2Int(-ablock[i].x, -ablock[i].y);
            ablock[i] = arotat;
        }
    }
}
