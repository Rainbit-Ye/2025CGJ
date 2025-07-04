using System.Collections.Generic;
using GamePlay;
using Music;
using UnityEngine;
using UnityEngine.EventSystems;

public class Feeding : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerMoveHandler
{
    [Header("计算上一次鼠标移动位置间隔时间，从而控制惯性")]
    public float lastTime;
    [Header("惯性")]
    public float force = 2;
    [Header("当前的食物图标")]
    public GameObject foodImg;
    [Header("鼠标跟随平滑度")]
    public float lerpSpeed;
    
    private Vector3 _mousePos;
    private Vector3 _lastMousePos;
    private int _foodIndex = 0;
    private GameManager _GameManager;
    private float _time;
    private bool _isEnter;
    private Dictionary<GameManager.MonsterType, Queue<GameObject>> _pools = new Dictionary<GameManager.MonsterType, Queue<GameObject>>();

    public static bool isEnable = true;
    void Start()
    {
        _GameManager = GameManager.Ins;
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Init();
    }

    private void Init()
    {
        
        foodImg.transform.position = new Vector3(_mousePos.x, _mousePos.y, 0);
        InvokeRepeating("RecordMousePos",0f, lastTime);
        FoodPool.Ins.CreateFoodPool();
        
        Sprite sprite = _GameManager.foodPbs[_foodIndex].GetComponent<Food>().foodSprite;
        foodImg.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isEnter)
        {
            if (Input.GetMouseButtonDown(0))
            {
                FoodDrop();
                MusicManager.Ins.MouseClick();
                return;
            }

            float t = Input.GetAxis("Mouse ScrollWheel");
            if (t != 0)
            {
                SwitchFood(t);
            }

            FollowMouse();
        }

        foodImg.SetActive(_isEnter);
    }

    #region 喂食UI范围控制

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isEnable)
        {
            _isEnter = true;
            foodImg.transform.position = new Vector3(_mousePos.x, _mousePos.y, 0);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isEnable)
        {
            _isEnter = false;
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        
    }

    #endregion

    #region 食物跟随以及生成
    private void FollowMouse()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 vector= Vector3.Lerp(foodImg.transform.position,_mousePos,lerpSpeed);
        foodImg.transform.position = new Vector3(vector.x, vector.y,0);
    }

    private void FoodDrop()
    {
        GameObject obj = FoodPool.Ins.GetFood((GameManager.MonsterType)_foodIndex);
        obj.transform.position = new Vector3(_mousePos.x, _mousePos.y, 0);

        Food food = obj.GetComponent<Food>();
        Vector3 dis = _mousePos - _lastMousePos;
        Rigidbody2D rb = food.RigidBody;
        rb.AddForce(new Vector2(dis.x,0) * force, ForceMode2D.Impulse);
        //我想写挂载一个有刚体的东西在这里生成预制体
    }

    private void SwitchFood(float t)
    {
        if (t < 0)
        {
            _foodIndex = (_foodIndex + 1) % _GameManager.foodPbs.Length;
        }
        else
        {
            _foodIndex = (_foodIndex - 1 + _GameManager.foodPbs.Length) % _GameManager.foodPbs.Length;
        }
        Sprite sprite = _GameManager.foodPbs[_foodIndex].GetComponent<Food>().foodSprite;
        foodImg.GetComponent<SpriteRenderer>().sprite = sprite;
    }



    private void RecordMousePos()
    {
        _lastMousePos = _mousePos;
    }
    #endregion
}