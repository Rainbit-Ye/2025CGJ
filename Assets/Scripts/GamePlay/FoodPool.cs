using System.Collections.Generic;
using Base;
using UnityEngine;

namespace GamePlay
{
    public class FoodPool : Singleton<FoodPool>
    {
        private Dictionary<GameManager.MonsterType, Queue<GameObject>> _pools = new Dictionary<GameManager.MonsterType, Queue<GameObject>>();
        public void CreateFoodPool()
        {
            GameManager g = GameManager.Ins;
            for (int i = 0; i < GameManager.Ins.TypeCount; i++)
            {
                // 为每种类型创建新队列
                var foodQueue = new Queue<GameObject>();
        
                // 填充指定数量的食物
                for (int j = 0; j < g.foodAmount; j++)
                {
                    GameObject obj = g.InstantiateObj(GameManager.Ins.foodPbs[i]);
                    obj.SetActive(false);
                    foodQueue.Enqueue(obj);
                }
                _pools.Add((GameManager.MonsterType)i, foodQueue);
            }
        }

        public GameObject GetFood(GameManager.MonsterType monsterType)
        {
            if (_pools.ContainsKey(monsterType))
            {
                GameObject obj = _pools[monsterType].Dequeue();
                Debug.Log(_pools[monsterType].Count);
                obj.SetActive(true);
                return obj;
            }
            Debug.Log("没东西了");
            return null;
        }

        public void QueueFood(GameManager.MonsterType monsterType, GameObject obj)
        {
            _pools[monsterType].Enqueue(obj);
        }
    }
}