using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay;
using UnityEngine;

public class StartController : MonoBehaviour
{
    public static bool _isStart = true;
    public GameObject whiteScreen;
    
    private Monster _monster;
    private bool _isEating = false;

    private void Start()
    {
        _monster = GetComponent<Monster>();
    }

    private void Update()
    {
        Debug.Log(_monster.EmoType);
        if (_monster.EmoType == Monster.MonsterEmo.Eating)
        {
            _isEating = true;
        }

        if (_monster.EmoType != Monster.MonsterEmo.Eating && _isEating)
        {
            whiteScreen.SetActive(false);
            UIManager.Ins.StartActor();
            this.gameObject.SetActive(false);
        }
    }
}
