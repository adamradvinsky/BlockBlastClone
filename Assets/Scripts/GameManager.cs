using TMPro;
using UnityEngine;

using System.Collections.Generic;
using Unity.Mathematics;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Playables;
using System.Linq;

public class GameManager : MonoBehaviour
{

    public TMP_Text scoreText;
    public GameObject screen;
    public GameObject lose_Screen;
    private int score;
    private int shapeCount = 3;
    private Vector3 originalScoreTextPos;
    public GridManager gridMan;


    public List<GameObject> blocks = new List<GameObject>();


    public List<GameObject> inGameBlocks = new List<GameObject>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        addShapes();
        originalScoreTextPos = scoreText.transform.position;

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
        float time = 0;
        while (score < newScore)
        {
            yield return new WaitForSeconds((float)0.1);
            time += 0.03f;
            float bruh = add * scoreCurve.Evaluate(time);
            score += (int)bruh;
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
    public void buttonPlayAgainPress()
    {
        Debug.Log("bruh");
        StartCoroutine(PlayAgain());
    }

    public IEnumerator Lose()
    {

        yield return new WaitForSeconds((float)0.2);
        Debug.Log("you lost");
        screen.GetComponent<Animator>().SetTrigger("lost");
        yield return new WaitForSeconds((float)0.5);
        lose_Screen.SetActive(true);
        scoreText.transform.position = new Vector3(0, 0, scoreText.transform.position.z);
        yield return null;
    }


    public IEnumerator PlayAgain()
    {
        yield return new WaitForSeconds((float)0.2);
        Debug.Log("playing again");
        // reset board and score
        gridMan.resetBoard();
        score = 0;
        screen.GetComponent<Animator>().SetTrigger("lost");
        yield return new WaitForSeconds((float)0.5);
        updateScore();
        foreach (GameObject shape in inGameBlocks.ToList())
        {
            inGameBlocks.Remove(shape);
            Destroy(shape);
        }
        shapeCount = 3;
        addShapes();

        lose_Screen.SetActive(false);
        scoreText.transform.position = originalScoreTextPos;

        yield return null;
    }


}
