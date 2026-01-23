using UnityEngine;

public class TileScript : MonoBehaviour
{
    int x, y;
    SpriteRenderer sr;
    public Sprite emptySprite;
    public Sprite fillSprite;
    public Animator anim;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = emptySprite;
    }

    public void Init(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void SetColor(Color c)
    {
        sr.color = c;
    }



    public void setToEmpty()
    {
        //sr.sprite = emptySprite;
        anim.SetBool("fill", false);
    }

    public void setToFill(Color colour)
    {
        //sr.sprite = fillSprite;

        this.transform.Find("Square").GetComponent<SpriteRenderer>().color = colour;
        anim.SetBool("fill", true);


    }

    public void setToHover(Color colour)
    {
        //sr.sprite = fillSprite;

        this.transform.Find("Square").GetComponent<SpriteRenderer>().color = Color.gray;
        anim.SetBool("hover", true);
    }

    public void settoClear()
    {
        anim.SetTrigger("clear");
    }


    public void setToNotHover()
    {
        //sr.sprite = fillSprite;

        anim.SetBool("hover", false);
    }

    public void setToClearHover()
    {
        //sr.sprite = fillSprite;

        anim.SetBool("clearhover", true);
    }

    public void setToNotClearHover()
    {
        //sr.sprite = fillSprite;

        anim.SetBool("clearhover", false);
    }

}