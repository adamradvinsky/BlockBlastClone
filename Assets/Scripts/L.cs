using System;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class L : shape
{

    public int[][] block = new int[3][];


    // public Transform transform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        block[0] = new int[] { 1, 1, 1 };

        block[1] = new int[] { 1, 0, 0 };

        block[2] = new int[] { 1, 0, 0 };



    }

    // Update is called once per frame
    void Update()
    {


    }






}
