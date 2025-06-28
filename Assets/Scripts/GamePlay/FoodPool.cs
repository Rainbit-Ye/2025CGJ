using System.Collections.Generic;
using Base;
using GamePlay;
using UnityEngine;
public class FoodPool : Singleton<FoodPool>
{
    private Dictionary<GameManagers.MonsterType, Queue<GameObject>> _pools = new Dictionary<GameManagers.MonsterType, Queue<GameObject>>();
    public void CreateFoodPool()
    {
        GameManagers g = GameManagers.Ins;
        for (int i = 0; i < GameManagers.Ins.TypeCount; i++)
        {
            // 为每种类型创建新队列
            var foodQueue = new Queue<GameObject>();
        
            // 填充指定数量的食物
            for (int j = 0; j < g.foodAmount; j++)
            {
                GameObject obj = g.InstantiateObj(GameManagers.Ins.foodPbs[i]);
                obj.SetActive(false);
                foodQueue.Enqueue(obj);
            }
            _pools.Add((GameManagers.MonsterType)i, foodQueue);
        }
    }

    public GameObject GetFood(GameManagers.MonsterType monsterType)
    {
        if (_pools.ContainsKey(monsterType))
        {
            GameObject obj = _pools[monsterType].Dequeue();
            obj.SetActive(true);
            return obj;
        }
        Debug.Log("没东西了");
        return null;
    }

    public void QueueFood(GameManagers.MonsterType monsterType, GameObject obj)
    {
        _pools[monsterType].Enqueue(obj);
    }
}