using UnityEngine;

namespace GamePlay
{
    public class Monster : MonoBehaviour
    {
        //todo 根据饥饿程度+变异程度来控制当前怪物的样子
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
            Hunger += value;
            if (type != this.monsterType)
            {
                _mutationRate += mutationRateValue;
                if (_mutationRate >= 1)
                {
                    //todo 后续会改成按概率变异
                    Debug.Log("变异了");
                }
            }
        }
        
    }
}
