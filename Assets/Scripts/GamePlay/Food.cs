using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GamePlay
{
    public class Food : MonoBehaviour
    {
        public GameManager.MonsterType monsterType;
        public Sprite foodSprite;
        public Rigidbody2D RigidBody
        {
            get { return _rb; }
            set { _rb = value; }
        }
        private Rigidbody2D _rb;
        private int _growNum;
        [Header("吃到回复饥饿值")]
        public float value;

        [Header("长成新植物的概率 0-1之间")]
        public float growNewPlantsRate;

        private void Awake()
        {
            RigidBody = GetComponent<Rigidbody2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag("Ground") || other.CompareTag("Monster"))
            {
                if (other.CompareTag("Monster"))
                {
                    Monster monster = other.gameObject.GetComponentInParent<Monster>();
                    monster.GetFood(value, monsterType);
                }

                if (other.CompareTag("Ground") && !StartController._isStart)
                {
                    GrowUpPlants();
                }
                this.gameObject.SetActive(false);
                FoodPool.Ins.QueueFood(monsterType, this.gameObject);
            }
        }

        private void GrowUpPlants()
        {
            float rand = Random.Range(0f, 1f);
            if (rand < growNewPlantsRate)
            {
                GameObject[] objs = GameManager.Ins.monsterPfb;
                int monsterNum = objs.Length;
                int randNum = Random.Range(0, monsterNum);
                GameObject obj = Instantiate(GameManager.Ins.monsterPfb[randNum], transform.position, Quaternion.identity, GameManager.Ins.plantPbsParent);
                obj.transform.SetParent(GameManager.Ins.plantPbsParent);
                GameManager.Ins.plantSortAmount += 6;
                obj.GetComponent<SpriteRenderer>().sortingOrder = GameManager.Ins.plantSortAmount;
            }
        }
        
    }
}