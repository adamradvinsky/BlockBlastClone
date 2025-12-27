using UnityEngine;

public class tileScript : MonoBehaviour
{


    private SpriteRenderer spriteRenderer;
    public GridManager gridManager;

    public int x = 0;
    public int y = 0;
    public int[][] block = new int[3][];


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    public void setX(int num)
    {
        x = num;
    }

    public void setY(int num)
    {
        y = num;
    }

    public void setColour(Color color)
    {

        spriteRenderer.color = color;
    }

    public void setOtherColour()
    {

        spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    void OnMouseDown()
    {

        // want to place block down at this position
        gridManager.placeAble(x, y, block);



    }



    void OnMouseOver()
    {

        if (gridManager.shape != GridManager.typeShape.nothing)
        {
            gridManager.hovering(x, y);

        }

    }


}
