using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

namespace Base
{
     /// <summary>
     /// 提供给外部添加帧更新事件的方法
     /// 提供给外部添加 协程的方法
     /// </summary>
     public class MonoMgr : SingletonAutoMono<MonoMgr>
     {
         private event UnityAction updateEvent;
     
         // Use this for initialization
         void Start () {
     
         }
 	    
         // Update is called once per frame
         void Update () {
             if (updateEvent != null)
                 updateEvent();
         }
     
         /// <summary>
         /// 添加Mono脚本中Update事件的函数
         /// </summary>
         /// <param name="fun"></param>
         public void AddUpdateListener(UnityAction fun)
         {
             updateEvent += fun;
         }
     
         /// <summary>
         /// 移除Mono脚本中Update事件的函数
         /// </summary>
         /// <param name="fun"></param>
         public void RemoveUpdateListener(UnityAction fun)
         {
             updateEvent -= fun;
         }
         
     }   
}

