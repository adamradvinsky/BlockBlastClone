using System;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class shape : MonoBehaviour
{

    public float test;
    public int[][] block = new int[3][];


    // public Transform transform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("bruh ");
        block[0] = new int[] { 1, 0 };

        block[1] = new int[] { 1, 1 };

        block[2] = new int[] { 1, 0 };



    }

    // Update is called once per frame
    void Update()
    {


    }


    void OnMouseDrag()
    {
        Vector2 drag_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(drag_position.x, drag_position.y, -9);
    }


    void OnMouseUp()
    {

        Debug.Log("bro it released");
        // delete this object
        //  

    }

    

}
