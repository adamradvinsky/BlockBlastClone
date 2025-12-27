using System;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class plus : MonoBehaviour
{

    public float test;
    private int[][] block = new int[3][];

    public GridManager gridManager;

    // public Transform transform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        block[0] = new int[] { 0, 1, 0 };

        block[1] = new int[] { 1, 1, 1 };

        block[2] = new int[] { 0, 1, 0 };



    }

    // Update is called once per frame
    void Update()
    {


    }


    void OnMouseDrag()
    {
        Vector2 drag_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(drag_position.x, drag_position.y, -9);
        gridManager.setBlock(block, GridManager.typeShape.plus);
        gridManager.shape = GridManager.typeShape.plus;

    }


    void OnMouseUp()
    {
        transform.position = new Vector2(-8, 0);
    }


}
