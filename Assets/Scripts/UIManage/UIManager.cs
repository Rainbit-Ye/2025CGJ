using System;
using System.Collections;
using System.Collections.Generic;
using Base;
using TMPro;
using UnityEngine;

public class UIManager : SingletonMono<UIManager>
{
    public GameObject finalScore;

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
    }

    /// <summary>
    /// 得分
    /// </summary>
    public void GetScore()
    {
        TotalScore += 100;
        _scoreText.text = TotalScore.ToString();
    }
}
