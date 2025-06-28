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
            Blue,
            Green,
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
            InvokeRepeating("AutoGroupPositions", 0f, monsterInterval);
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

        public Coroutine TimerBegin(GameObject obj,float waitTime)
        {
            Coroutine coroutine = StartCoroutine(Timer(obj, waitTime));
            return coroutine;
        }

        public void TimerEnd(Coroutine coroutine)
        {
            StopCoroutine(coroutine);
        }

        public void RefashTimer(GameObject obj,float waitTime,Coroutine coroutine)
        {
            if(coroutine != null)
                StopCoroutine(coroutine);
            TimerBegin(obj, waitTime);
        }
        
        private IEnumerator Timer(GameObject obj,float waitTime)
        {
            obj.SetActive(true);
            yield return new WaitForSeconds(waitTime);
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
}
