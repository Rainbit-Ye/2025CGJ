using System;
using UnityEngine;

namespace GamePlay
{
    public class Food : MonoBehaviour
    {
        public GameManagers.MonsterType monsterType;

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log(other.gameObject.name);
            if (other.CompareTag("Monster"))
            {
                Destroy(this.gameObject);
            }
        }
        
    }
}
