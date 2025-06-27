using System;
using UnityEngine;

namespace GamePlay
{
    public class Food : MonoBehaviour
    {
        public GameManagers.MonsterType monsterType;
        [Header("吃到回复饥饿值")]
        public float value;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Monster"))
            {
                Monster monster = other.gameObject.GetComponentInParent<Monster>();
                monster.GetFood(value,monsterType);
                Destroy(this.gameObject);
            }
        }
        
    }
}
