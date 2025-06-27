using UnityEngine;

namespace GamePlay
{
    public class Monster : MonoBehaviour
    {
        public GameManagers.MonsterType monsterType;
        [Header("最大饱食度")]
        public float maxHunger;
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
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
