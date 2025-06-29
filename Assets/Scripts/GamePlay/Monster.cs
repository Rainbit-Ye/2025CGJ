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
        public enum MonsterEmo
        {
            Eating,
            Normal,
            Hungry,
        }
        
        
        public GameManager.MonsterType monsterType;
        [Header("最大饱食度")]
        public float maxHunger;
        [Header("下降到什么时候开始提示饥饿值")]
        public float hungerNotion;
        [Header("间隔多久下降饥饿")]
        public float hungerInterval;
        [Header("每次下降多少饥饿值 x:最小，y：最大")]
        public Vector2 hungerRateValue;
        [Header("吃到错误的食物增加的变异程度")]
        public float mutationRateValue;
        [Header("喂错变得饥饿更快（间隔下降饥饿时间减少多少）")]
        public float hungerReduceTime;

        [Header("心情气泡是Image 上面还有一个子类")]
        public SpriteRenderer emoBubbleTip;
        [Header("心情气泡弹出时间")]
        public float bubbleTipTime;
        [Header("饥饿倒计时")]
        public GameObject hungerSlider;
        
        [Header("心情图片")]
        public Sprite[] emoSprite;
        

        public MonsterEmo EmoType
        {
            get { return _currentEmo;}
        }
        private Animator _stateAnim;
        private float _mutationRate;
        private float _hunger;
        private MonsterMutation _currentMutation;
        private float _time;
        private Coroutine _hungerCoroutine;
        private bool _isEating = false;
        private MonsterEmo _currentEmo = MonsterEmo.Normal;
        private Shader _shader;
        private Material _material;
        private GameObject _bubbleParent;
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
            //InvokeRepeating("ReduceHunger",hungerInterval, hungerInterval);
            Hunger = maxHunger;
            _shader = Shader.Find("Custom/Circle");
            _material = new Material(_shader);
            SpriteRenderer sr = hungerSlider.GetComponent<SpriteRenderer>();
            sr.material = _material;
            _bubbleParent = emoBubbleTip.transform.parent.gameObject;
            _stateAnim = GetComponent<Animator>();
        }

        private void Update()
        {
            UpdateRefreshTimer();
        }

        private void ReduceHunger()
        {
            if (_currentEmo == MonsterEmo.Eating)
            {
                hungerInterval -= 3;
                _isEating = false;
                _currentEmo = MonsterEmo.Normal;
            }
            float hungerReduce = Random.Range(hungerRateValue.x, hungerRateValue.y);
            Hunger -= hungerReduce;
            //进入到饥饿状态
            if (Hunger <= hungerNotion)
            {
                _currentEmo = MonsterEmo.Hungry;
                SetEmo(0);
                _bubbleParent.SetActive(true);
                hungerSlider.SetActive(true);
                SliderGetDown();
            }
            // else
            // {
            //     if(_currentEmo == MonsterEmo.Hungry)
            //     _currentEmo = MonsterEmo.Normal;
            // }
            EmoUpDate();
            if (Hunger <= 0)
            {
                Death();
            }
        }

        private void Death()
        {
            this.gameObject.SetActive(false);
            UIManager.Ins.EndActor();
            //Destroy(this.gameObject);
        }

        public void GetFood(float value,GameManager.MonsterType type)
        {
            if(_currentEmo == MonsterEmo.Eating) return;
            Eating();
            MusicManager.Ins.MonsterEat();
            if (type != this.monsterType)
            {
                _mutationRate += mutationRateValue * ((int)_currentMutation + 1);
                if (hungerInterval > 3)
                {
                    hungerInterval -= hungerReduceTime;
                }
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
                SliderGetDown();
                if (type != this.monsterType)
                {
                    _bubbleParent.SetActive(true);
                    GameManager.Ins.RefashTimer(this,bubbleTipTime,_hungerCoroutine,2);
                }
                else
                {
                    _bubbleParent.SetActive(true);
                    UIManager.Ins.GetScore();
                    GameManager.Ins.RefashTimer(this,bubbleTipTime,_hungerCoroutine,1);
                }
                _hungerCoroutine = null;
                if (Hunger > hungerNotion)
                {
                    hungerSlider.SetActive(false);
                    //emoBubbleTip.SetActive(false);
                }

                if (Hunger > maxHunger)
                {
                    Hunger = maxHunger;
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
                UIManager.Ins.EndActor();
            }
            if (_currentMutation == MonsterMutation.High)
            {
                MusicManager.Ins.PlayBackgroundMusic(1);
            }
            MutationUpDate();
        }
        #endregion

        /// <summary>
        /// 进度条减少计算
        /// </summary>
        private void SliderGetDown()
        {
            _material.SetColor("_Color",new Color(0.694f, 0.710f, 0.710f));
            _material.SetFloat("_Progress", 1 - (float)(Hunger / maxHunger));
        }

        private void Eating()
        {
            if (!_isEating)
            {
                _isEating = true;
                hungerInterval += 3; //增加三秒处于进食状态
                //进入到咀嚼状态
                _currentEmo = MonsterEmo.Eating;
                EmoUpDate();
            }
        }

        /// <summary>
        /// 变异状态检测,用来动作切换
        /// </summary>
        private void MutationUpDate()
        {
            switch (_currentMutation)
            {
                case MonsterMutation.Normal:
                    _stateAnim.SetInteger("MonsterLevel",0);
                    break;
                case MonsterMutation.Middle:
                    _stateAnim.SetInteger("MonsterLevel",1);
                    break;
                case MonsterMutation.High:
                    _stateAnim.SetInteger("MonsterLevel",2);
                    break;
            }
        }

        /// <summary>
        /// 0-2 0：饥饿 1：高兴 2：悲伤
        /// </summary>
        /// <param name="index"></param>
        public void SetEmo(int index)
        {
            emoBubbleTip.sprite = emoSprite[index];
            //_stateAnim.SetInteger("MonsterEmo",index);
        }
        
        private void EmoUpDate()
        {
            Debug.Log(_currentEmo.ToString());
            switch (_currentEmo)
            {
                case MonsterEmo.Eating:
                    _stateAnim.SetInteger("MonsterEmo",0);
                    break;
                case MonsterEmo.Normal:
                    _stateAnim.SetInteger("MonsterEmo",1);
                    break;
                case MonsterEmo.Hungry:
                    _stateAnim.SetInteger("MonsterEmo",2);
                    break;
            }
        }

        private void UpdateRefreshTimer()
        {
            if (_time > hungerInterval)
            {
                ReduceHunger();
                _time = 0;
            }
            AddTime(ref _time);
        }
        
        private void AddTime(ref float time)
        {
            time += Time.deltaTime;
        }

        public void TurnToNormal()
        {
            // _currentEmo = MonsterEmo.Normal;
            // EmoUpDate();
        }
    }
}