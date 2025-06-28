using System;
using Music;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GamePlay
{
    public class Monster : MonoBehaviour
    {
        //todo 饥饿条
        private enum MonsterMutation
        {
            Normal,
            Middle,
            High,
        }
        private enum MonsterEmo
        {
            Hungry,
            Eating,
            Normal,
        }
        
        
        public GameManager.MonsterType monsterType;
        [Header("最大饱食度")]
        public float maxHunger;
        [Header("下降到什么时候开始提示饥饿值")]
        public float hungerNotion;
        [Header("间隔多久下降饥饿")]
        public float hungerInterval;
        [Header("每次下降多少饥饿值")]
        public float hungerRateValue;
        [Header("吃到错误的食物增加的变异程度")]
        public float mutationRateValue;

        [Header("心情气泡Prefabs 也可能是Image")]
        public GameObject emoBubbleTip;
        [Header("心情气泡弹出时间")]
        public float bubbleTipTime;
        [Header("饥饿倒计时")]
        public GameObject hungerSlider;
        
        [Header("心情图片")]
        public Sprite[] emoSprite;
        private float _mutationRate;
        private float _hunger;
        private MonsterMutation _currentMutation;
        private float _time;
        private Coroutine _hungerCoroutine;
        private bool _isEating = false;
        private MonsterEmo _currentEmo = MonsterEmo.Normal;
        #region 怪物行为状态
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
        
        private void ReduceHunger()
        {
            if (_currentEmo == MonsterEmo.Eating)
            {
                hungerInterval -= 3;
                _isEating = false;
                _currentEmo = MonsterEmo.Normal;
            }
            Hunger -= hungerRateValue;
            //进入到饥饿状态
            if (Hunger <= hungerNotion)
            {
                _currentEmo = MonsterEmo.Hungry;
                _hungerCoroutine = GameManager.Ins.TimerBegin(emoBubbleTip,bubbleTipTime);
                emoBubbleTip.GetComponent<SpriteRenderer>().sprite = emoSprite[0];
                hungerSlider.SetActive(true);
                SliderGetDown(hungerSlider);
            }
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

        public void GetFood(float value,GameManager.MonsterType type)
        {
            Eating();
            //MusicManager.Ins.MonsterEat();
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
                SliderGetDown(hungerSlider);
                if (type != this.monsterType)
                {
                    emoBubbleTip.GetComponent<SpriteRenderer>().sprite = emoSprite[2];
                }
                else
                {
                    UIManager.Ins.GetScore();
                    emoBubbleTip.GetComponent<SpriteRenderer>().sprite = emoSprite[1];
                }
                //todo 有miss错误
                GameManager.Ins.RefashTimer(emoBubbleTip, bubbleTipTime, _hungerCoroutine);
                _hungerCoroutine = null;
                if (Hunger > hungerNotion)
                {
                    hungerSlider.SetActive(false);
                }
                return;
            }
            Hunger = maxHunger;
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
        #endregion

        /// <summary>
        /// 进度条减少计算
        /// </summary>
        private void SliderGetDown(GameObject slider)
        {
            SpriteRenderer spriteRenderer = slider.GetComponent<SpriteRenderer>();
            spriteRenderer.size = new Vector2(Hunger / maxHunger, spriteRenderer.size.y);
        }

        private void Eating()
        {
            if (!_isEating)
            {
                _isEating = true;
                hungerInterval += 3; //增加三秒处于进食状态
                //进入到咀嚼状态
                _currentEmo = MonsterEmo.Eating;
            }
        }

        /// <summary>
        /// 心情状态检测,用来动作切换
        /// </summary>
        private void EmoUpDate()
        {
            
        }

    }
}