
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public int radius = 8;

    public GameObject tile;

    Vector2 current;

    public int[,] grid;
    public GameObject[,] gameGrid;
    private int[][] block = new int[3][];
    private int[][] prevBlock = new int[3][];


    public enum typeShape
    {
        tetris,
        L,
        plus,
        nothing
    }


    public typeShape shape;

    void Start()
    {


        prevBlock[0] = new int[] { 0, 0, 0 };

        prevBlock[1] = new int[] { 0, 0, 0 };

        prevBlock[2] = new int[] { 0, 0, 0 };



        grid = new int[radius, radius];
        gameGrid = new GameObject[radius, radius];


        shape = typeShape.nothing;


        // goes down
        for (int j = 0; j < radius; j++)
        {
            // prints right
            for (int i = 0; i < radius; i++)
            {

                gameGrid[i, j] = Instantiate(tile, current, Quaternion.identity);
                GameObject newTile = gameGrid[i, j];
                newTile.transform.position = new Vector2(transform.position.x + (newTile.transform.localScale.x * i), transform.position.y - (newTile.transform.localScale.y * j));
                newTile.GetComponent<tileScript>().setX(i);
                newTile.GetComponent<tileScript>().setY(j);

                grid[i, j] = 0;
            }

        }
    }

    public void setBlock(int[][] shapeGrid, typeShape type)
    {
        shape = type;

        equalArray(block, shapeGrid, 3);
    }

    public void placeAble(int x, int y, int[][] block)
    {


        bool good = true;

        if (x + 3 > radius || y + 3 > radius)
        {
            return;
        }

        for (int j = 0; j < block.Length; j++)
        {
            // prints right
            for (int i = 0; i < block[1].Length; i++)
            {
                if (block[j][i] == 1 && grid[x + i, y - j] != 0)
                {
                    good = false;
                }
            }
        }


        if (good)
        {
            setTile(x, y, block);
        }
        else
        {
            // cant place a block
            Debug.Log("cant place a block there");

        }
    }


    int prevX = 0;
    int prevY = 0;

    public void hovering(int x, int y)
    {

        if (prevX == x && prevY == y)
        {
            // set prevblock to the block thats currently active

            equalArray(prevBlock, block, 3);

            return;
        }

        // delete prev thing
        draw(prevX, prevY, prevBlock, new Color(1.0f, 1.0f, 1.0f, 1.0f));

        // draw new thing
        draw(x, y, prevBlock, new Color(1.0f, 0f, 0f, 1.0f));


        // what if at start calculates which tiles to draw and which to make white

        // creates an array that takes up entire space of old place and new place 
        // and calculates which ones to clean up and which to turn white
        prevX = x;
        prevY = y;

    }

    private void draw(int x, int y, int[][] block, Color color)
    {
        for (int j = 0; j < block.Length; j++)
        {
            // prints right
            for (int i = 0; i < block[1].Length; i++)
            {
                if (block[j][i] == 1)
                {
                    // prev block is white
                    gameGrid[x + i, y - j].GetComponent<tileScript>().setColour(color);
                }


            }
        }
    }

    private void equalArray(int[][] a, int[][] b, int len)
    {
        for (int i = 0; i < len; i++)
        {
            a[i] = new int[len];
            for (int j = 0; j < len; j++)
            {
                a[i][j] = b[i][j];
            }
        }
    }




    private void setTile(int x, int y, int[][] block)
    {
        for (int j = 0; j < block.Length; j++)
        {
            // prints right
            for (int i = 0; i < block[1].Length; i++)
            {
                if (block[j][i] == 1)
                {
                    gameGrid[x + i, y - j].GetComponent<tileScript>().setColour(new Color(1.0f, 1.0f, 1.0f, 1.0f));
                    grid[x + i, y - j] = 1;
                }
            }
        }
    }


}
