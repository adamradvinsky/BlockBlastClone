using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public TMP_Text scoreText;
    private int score;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addScore(int add)
    {
        score += add;
        updateScore();
    }


    public void updateScore()
    {
        scoreText.text = score.ToString();
    }


}
