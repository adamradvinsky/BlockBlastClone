using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class shape : MonoBehaviour
{

    public float test;
   // public Transform transform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("bruh ");

        //position = transform.GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        

    }


    void OnMouseDrag()
    {
        Vector2 drag_position= Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = drag_position;
    }

}
