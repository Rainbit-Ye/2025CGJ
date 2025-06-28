using System;
using System.Collections;
using System.Collections.Generic;
using Base;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace GamePlay
{
    public class GameManager : SingletonMono<GameManager>
    {
        public enum MonsterType
        {
            Meat,
            Finger,
        }
        public readonly int TypeCount = Enum.GetValues(typeof(MonsterType)).Length;
        [Header("怪物预制体")]
        public GameObject[] monsterPfb;
        [Header("编号以及生成位置")]
        public List<PositionGroup> positionGroups = new List<PositionGroup>();
        [Header("生成怪物时间间隔")]
        public float monsterInterval;
        [Header("食物预制体")]
        public GameObject[] foodPbs;
        [Header("每种食物的对象池个数")]
        public int foodAmount;
        [Header("存放对象池Tansformer")]
        public Transform foodPbsParent;
        
        private int _index = 0;
        private int _groupsNum;
        
        private void Start()
        {
            _groupsNum = positionGroups.Count;
            InvokeRepeating("AutoGroupPositions", monsterInterval, monsterInterval);
            //Init(0, 2);
            //Init(1,1);
        }

        private void Init(int groupsNum, int tfNum)
        {
            int monsterRandomIndex = Random.Range(0, monsterPfb.Length);
            Instantiate(monsterPfb[monsterRandomIndex], positionGroups[groupsNum].monsterTransforms[tfNum].transform.position, Quaternion.identity);
            var monsterTransform =  positionGroups[groupsNum].monsterTransforms[tfNum];
            monsterTransform.IsSpawned = true;
            positionGroups[groupsNum].monsterTransforms[tfNum] = monsterTransform;
            
        }

        #region 位置生成
        /// <summary>
        /// 自动生成位置
        /// </summary>
        public void AutoGroupPositions()
        {
            var currentGroup = positionGroups[_index];
            int monsterRandomIndex = Random.Range(0, monsterPfb.Length);
            
            for (int i = 0; i < currentGroup.monsterTransforms.Count; i++)
            {
                if (!currentGroup.monsterTransforms[i].IsSpawned)
                {
                    GameObject monster = Instantiate(monsterPfb[monsterRandomIndex], currentGroup.monsterTransforms[i].transform.position, Quaternion.identity);
                    var monsterTransform = currentGroup.monsterTransforms[i];
                    monsterTransform.IsSpawned = true;
                    currentGroup.monsterTransforms[i] = monsterTransform;
                    
                    positionGroups[_index] = currentGroup;
                    break;
                }
            }
            if (_index == _groupsNum - 1)
            {
                _index = 0;
            }
            else
            {
                _index++;
            }
        }

        [System.Serializable]
        public struct PositionGroup
        {
            public int groupIndex;
            public List<MonsterTransform> monsterTransforms;
        }
        [System.Serializable]
        public struct MonsterTransform
        {
            public Transform transform;
            public bool IsSpawned
            {
                get => _isSpawned;
                set => _isSpawned = value;
            }
            private bool _isSpawned;
            
        }
        #endregion

        public GameObject InstantiateObj(GameObject prefab)
        {
            GameObject obj = Instantiate(prefab, foodPbsParent);
            return obj;
        }

        public Coroutine TimerBegin(Monster monster,float waitTime,int index)
        {
            Coroutine coroutine = StartCoroutine(Timer(monster, waitTime,index));
            return coroutine;
        }

        public void TimerEnd(Coroutine coroutine)
        {
            StopCoroutine(coroutine);
        }

        public void RefashTimer(Monster monster,float waitTime,Coroutine coroutine,int index)
        {
            if(coroutine != null)
                StopCoroutine(coroutine);
            TimerBegin(monster, waitTime,index);
        }
        
        private IEnumerator Timer(Monster monster,float waitTime,int index)
        {
            monster.SetEmo(index);
            yield return new WaitForSeconds(waitTime);
            if (monster.EmoType.Equals(Monster.MonsterEmo.Hungry))
            {
                monster.SetEmo(0);
            }
            else
            {
                monster.emoBubbleTip.transform.parent.gameObject.SetActive(false);
            }
            
        }
    }
}
