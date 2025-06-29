using System;
using System.Collections;
using System.Collections.Generic;
using Base;
using GamePlay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMono<UIManager>
{
    [Header("最终得分")]
    public GameObject finalScore;
    [Header("开始按钮")]
    public Button startButton;

    [Header("GamingUI")] 
    public GameObject gamingUI;

    [Header("开始拖拽UI")]
    public GameObject startUI;
    public int TotalScore
    {
        get { return _totalScore; }
        set { _totalScore = value; }
    }
    private int _totalScore;//玩家得分
    private TextMeshProUGUI _scoreText;

    private void Start()
    {
        _scoreText = finalScore.GetComponent<TextMeshProUGUI>();
        startButton.onClick.AddListener(GameManager.Ins.GameStart);
    }

    public void InitUI()
    {
        _scoreText.text = "";
    }

    /// <summary>
    /// 得分
    /// </summary>
    public void GetScore()
    {
        TotalScore += 100;
        _scoreText.text = TotalScore.ToString();
    }

    public void StartActor()
    {
        startUI.SetActive(false);
        gamingUI.SetActive(true);
    }
}
