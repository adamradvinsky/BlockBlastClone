
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public int radius = 8;

    public GameObject tile;

    Vector2 current;


    void Start()
    {

        current = transform.position;

        // goes right
        for (int i = 0; i < radius; i++)
        {
            // goes down
            for (int j = 0; j < radius; j++)
            {
                current = new Vector2(tile.transform.localScale.x * i, tile.transform.localScale.y * j);
                Instantiate(tile, current, Quaternion.identity);
            }
        }


    }

    // Update is called once per frame
    void Update()
    {

    }
}
