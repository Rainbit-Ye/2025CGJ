using System.Collections.Generic;
using Base;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GamePlay
{
    public class GameManagers : SingletonAutoMono<GameManagers>
    {
        public enum MonsterType
        {
            Blue,
            Green,
        }
        [Header("怪物预制体")]
        public GameObject[] monsterPfb;
        [Header("编号以及生成位置")]
        public List<PositionGroup> positionGroups = new List<PositionGroup>();
        [Header("生成怪物时间间隔")]
        public float monsterInterval;
        private int _index = 0;
        private int _groupsNum;
        private void Start()
        {
            _groupsNum = positionGroups.Count;
            InvokeRepeating("AutoGroupPositions", 0, monsterInterval);
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


    }
}
