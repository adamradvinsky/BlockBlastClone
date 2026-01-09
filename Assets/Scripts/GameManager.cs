using TMPro;
using UnityEngine;

using System.Collections.Generic;
using Unity.Mathematics;

public class GameManager : MonoBehaviour
{

    public TMP_Text scoreText;
    private int score;
    private int shapeCount = 3;


    public List<GameObject> blocks = new List<GameObject>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        addShapes();
    }

    public void shapeCountDecrease()
    {
        shapeCount--;
        if (shapeCount <= 0)
        {
            addShapes();
            shapeCount = 3;
        }
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



    public Vector3Int[] startPositions = new Vector3Int[]
    {
        // collumn : row
        new Vector3Int(0, 0)
    };


    public void addShapes()
    {
        int random = 0;
        for (int i = 0; i < 3; i++)
        {
            random = UnityEngine.Random.Range(0, blocks.Count);
            Instantiate(blocks[random], startPositions[i], quaternion.identity);
        }

    }


}
