using System;
using UnityEngine;

namespace GamePlay
{
    public class Food : MonoBehaviour
    {
        public GameManagers.MonsterType monsterType;
        public Sprite foodSprite;
        public Rigidbody2D RigidBody
        {
            get { return _rb; }
            set { _rb = value; }
        }
        private Rigidbody2D _rb;
        
        [Header("吃到回复饥饿值")]
        public float value;

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
                this.gameObject.SetActive(false);
                FoodPool.Ins.QueueFood(monsterType, this.gameObject);
            }
        }
        
    }
}