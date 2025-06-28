using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GamePlay
{
    public class Monster : MonoBehaviour
    {
        //todo 根据饥饿程度+变异程度来控制当前怪物的样子
        private enum MonsterMutation
        {
            Low,
            Middle,
            High,
        }
        private enum MonsterEmo
        {
            Happy,
            Sad,
        }
        
        public GameManagers.MonsterType monsterType;
        [Header("最大饱食度")]
        public float maxHunger;
        [Header("间隔多久下降饥饿")]
        public float hungerInterval;
        [Header("每次下降多少饥饿值")]
        public float hungerRateValue;
        [Header("吃到错误的食物增加的变异程度")]
        public float mutationRateValue;
        [SerializeField]private float _mutationRate;
        private float _hunger;
        private MonsterMutation _currentMutation;
        
        //当前饱食度
        public float Hunger
        {
            get { return _hunger; }
            set { _hunger = value; }
        }
        // Start is called before the first frame update
        void Start()
        {
            InvokeRepeating("ReduceHunger",hungerInterval, hungerInterval);
            Hunger = maxHunger;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void ReduceHunger()
        {
            Hunger -= hungerRateValue;
            if (Hunger <= 0)
            {
                Death();
            }
        }

        private void Death()
        {
            Debug.Log($"{monsterType} 饿死了");
            Destroy(this.gameObject);
        }

        public void GetFood(float value,GameManagers.MonsterType type)
        {
            if (type != this.monsterType)
            {
                _mutationRate += mutationRateValue;
                Debug.Log("增加变异可能性");
                float rand = Random.Range(0f, 1f);
                if (rand < _mutationRate)
                {
                    Mutation();
                    Debug.Log($"变异到{_currentMutation}");
                }
            }
            if (Hunger < maxHunger)
            {
                Hunger += value;
                return;
            }
            Debug.Log("吃饱了");
        }

        /// <summary>
        /// 控制变异程度
        /// </summary>
        public void Mutation()
        {
            if (_currentMutation != MonsterMutation.High)
            {
                _mutationRate = 0;
                _currentMutation = (MonsterMutation)((int)_currentMutation + 1);
                //todo 可以在这里切换对应形态
            }
            else
            {
                Debug.Log("吃人啦");
            }
        }
    }
}