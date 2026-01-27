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
using Unity.VectorGraphics;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public TMP_Text scoreText;
    public GameObject screen;
    public GameObject lose_Screen;
    private int score = 0;
    private int shapeCount = 3;
    private Vector3 originalScoreTextPos;
    public GridManager gridMan;
    public Canvas canvas;


    public List<GameObject> blocks = new List<GameObject>();


    public List<GameObject> inGameBlocks = new List<GameObject>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(canvas);
            DontDestroyOnLoad(scoreText);
            DontDestroyOnLoad(screen);
            DontDestroyOnLoad(lose_Screen);
            lose_Screen.SetActive(false);
            scoreText.enabled = false;

            Debug.Log("should be chillin");
        }
        else
        {
            Destroy(gameObject);
        }

    }





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {


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
        Debug.Log(blocks.Count);
        for (int i = 0; i < 3; i++)
        {
            random = UnityEngine.Random.Range(0, blocks.Count);
            GameObject newBlock = Instantiate(blocks[random], transform);
            newBlock.transform.position = startPositions[i].transform.position;
            inGameBlocks.Add(newBlock);
            //newBlock.transform.localScale = new Vector3(transform.localScale.x / 2, transform.localScale.y / 2, transform.localScale.z);
        }

        shapeCount = 3;

    }

    public void removeBlockFromGame(GameObject block)
    {
        inGameBlocks.Remove(block);
    }


    public IEnumerator Lose()
    {

        yield return new WaitForSeconds((float)0.2);
        Debug.Log("you lost");
        screen.GetComponent<Animator>().SetTrigger("lost");
        yield return new WaitForSeconds((float)0.5);
        lose_Screen.SetActive(true);
        scoreText.transform.position = new Vector3(Screen.width/2, Screen.height/2, scoreText.transform.position.z);
        //scoreText.GetComponent<RectTransform>().position = new Vector3(0, 0, scoreText.transform.position.z);
        yield return null;
    }



    public IEnumerator PlayAgain()
    {
        yield return new WaitForSeconds((float)0.2);
        Debug.Log("playing again");
        gridMan.lose = false;
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


    public void buttonPlayAgainPress()
    {
        StartCoroutine(PlayAgain());
    }

    public void startGame()
    {
        StartCoroutine(StartGameCo());
    }

    public IEnumerator StartGameCo() // Change from void to IEnumerator
    {
        Debug.Log("starting this");
        screen.GetComponent<Animator>().SetTrigger("lost");
        yield return new WaitForSeconds((float)0.5);
        SceneManager.LoadScene("game");
        lose_Screen.SetActive(false);
        scoreText.enabled = true;
        yield return null;

        //GameObject textGO = GameObject.FindWithTag("SCORE");
        //scoreText = textGO.GetComponent<TextMeshProUGUI>();


        //screen = GameObject.FindWithTag("SCREEN");
        //lose_Screen = GameObject.FindWithTag("LOSE");



        GameObject gridManagerGO = GameObject.FindWithTag("GridManager");
        gridMan = gridManagerGO.GetComponent<GridManager>();


        GameObject blockHolder = GameObject.FindWithTag("blockHolder");
        //disable start shit

        foreach (Transform blockTransform in blockHolder.transform)
        {
            GameObject childObject = blockTransform.gameObject;
            blocks.Add(childObject);
        }

        gridMan.setUpGrid();
        addShapes();
        originalScoreTextPos = scoreText.transform.position;




        Debug.Log("bruh?");
        yield return null;
    }


}
