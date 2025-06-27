using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feeding : MonoBehaviour
{
    [Header("食物预制体")]
    public GameObject[] foodPrbs;
    [Header("计算上一次鼠标移动位置间隔时间，从而控制惯性")]
    public float lastTime;
    [Header("惯性")]
    public float force = 2;
    
    private Vector3 _mousePos;

    private int _foodIndex = 0;
    private Vector3 _lastMousePos;

    private float _time;
    // Start is called before the first frame update
    void Start()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(_mousePos.x, _mousePos.y, 0);
        InvokeRepeating("RecordMousePos",0f, lastTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FoodDrop();
            return;
        }

        float t = Input.GetAxis("Mouse ScrollWheel");
        if (t != 0)
        {
            SwitchFood(t);
        }

        FollowMouse();
    }

    private void FollowMouse()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(_mousePos.x, _mousePos.y, 0);
    }

    private void FoodDrop()
    {
        GameObject food = Instantiate(foodPrbs[_foodIndex], transform.position, Quaternion.identity);
        Vector3 dis = _mousePos - _lastMousePos;
        Rigidbody2D rb = food.GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(dis.x,0) * force, ForceMode2D.Impulse);
        //我想写挂载一个有刚体的东西在这里生成预制体
    }

    private void SwitchFood(float t)
    {
        if (t < 0)
        {
            _foodIndex = (_foodIndex + 1) % foodPrbs.Length;
        }
        else
        {
            _foodIndex = (_foodIndex - 1 + foodPrbs.Length) % foodPrbs.Length;
        }
    }

    private void AddTime(ref float time)
    {
        time += Time.deltaTime;
    }

    private void RecordMousePos()
    {
        Debug.Log("_mousePos");
        _lastMousePos = _mousePos;
    }
}
