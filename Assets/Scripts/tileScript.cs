using UnityEngine;

public class TileScript : MonoBehaviour
{
    int x, y;
    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
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

    public void OnMouseDown()
    {
      //  Debug.Log( " x : "  + x + " y : " + y);
    }
}