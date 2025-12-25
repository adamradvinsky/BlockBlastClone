
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


    void Start()
    {
        grid = new int[radius, radius];
        gameGrid = new GameObject[radius, radius];

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

    public void placeAble(int x, int y, int[][] block)
    {
        bool good = true;

        for (int j = 0; j < block.Length; j++)
        {
            // prints right
            for (int i = 0; i < block[1].Length; i++)
            {

                if (x + i >= radius || y - j >= radius)
                {
                    Debug.Log("thatas out of the area dumbass");
                }
                else if (block[j][i] == 1 && grid[x + i, y - j] != 0)
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



    private void setTile(int x, int y, int[][] block)
    {
        for (int j = 0; j < block.Length; j++)
        {
            // prints right
            for (int i = 0; i < block[1].Length; i++)
            {
                if (block[j][i] == 1)
                {
                    gameGrid[x + i, y - j].GetComponent<tileScript>().setColour();
                    grid[x + i, y - j] = 1;
                }
            }
        }
    }


}
