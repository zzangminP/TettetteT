using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    
    // 증감식 -> GetBlockIndex에 로드씬에 있음
    public int currentStage = 1;

    // 내가 고를 블럭
    public int[] choosedTetroIndex = new int[7];



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }

        Application.targetFrameRate = 144;
    }
    void Start()
    {

        for (int i = 0; i < choosedTetroIndex.Length; i++)
        {
            choosedTetroIndex[i] = i;
            Debug.Log(choosedTetroIndex[i]);
        }

    }



    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 실행할꺼
        Init();
    }

    public void Init()
    {
        currentStage = 1;
        for (int i = 0; i < choosedTetroIndex.Length; i++)
        {
            choosedTetroIndex[i] = i;
            //Debug.Log(choosedTetroIndex[i]);
        }
    }


    /*
    void WhatStage()
    {

    }*/
}
