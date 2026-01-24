using TMPro;
using UnityEngine;

using System.Collections.Generic;
using Unity.Mathematics;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public TMP_Text scoreText;
    public GameObject screen;
    public GameObject lose_Screen;
    private int score;
    private int shapeCount = 3;


    public List<GameObject> blocks = new List<GameObject>();


    private List<GameObject> inGameBlocks = new List<GameObject>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        addShapes();
    }

    public List<GameObject> getInGameBlocks()
    {
        return inGameBlocks;
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

    public AnimationCurve scoreCurve;



    public IEnumerator addScore(int add)
    {
        int newScore = score + add;
        while (score < newScore)
        {
        yield return new WaitForSeconds((float)0.2);
            score += 3;
            updateScore();
        }
        yield return null;
    }


    public void updateScore()
    {
        
        scoreText.text = score.ToString();
    }



    public GameObject[] startPositions = new GameObject[]
    {
    };


    public void addShapes()
    {
        int random = 0;
        for (int i = 0; i < 3; i++)
        {
            random = UnityEngine.Random.Range(0, blocks.Count);
            GameObject newBlock = Instantiate(blocks[random], transform);
            newBlock.transform.position = startPositions[i].transform.position;


            inGameBlocks.Add(newBlock);
            //newBlock.transform.localScale = new Vector3(transform.localScale.x / 2, transform.localScale.y / 2, transform.localScale.z);
        }

    }

    public void removeBlockFromGame(GameObject block)
    {
        inGameBlocks.Remove(block);
    }

    // public void lose()
    // {
    //     Debug.Log("you lost");
    //     screen.GetComponent<Animator>().SetTrigger("lost");
    //     lose_Screen.SetActive(true);
    //     // set lose screen to active
    // }

    public IEnumerator Lose()
    {

        yield return new WaitForSeconds((float)0.2);
        Debug.Log("you lost");
        screen.GetComponent<Animator>().SetTrigger("lost");
        yield return new WaitForSeconds((float)0.5);
        lose_Screen.SetActive(true);
        yield return null;
    }


}
