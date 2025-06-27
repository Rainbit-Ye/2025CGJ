using UnityEngine;

namespace Base
{
    /// <summary>
    /// 实现单例 Mono 继承此类即可
    /// 原理是通过Awake添加静态实例
    /// 如果需要自动挂载在场景上 可以继承 SingletonAutoMono
    /// </summary>
    /// <typeparam name="T">需继承MonoBehaviour</typeparam>
    public class SingletonMono<T> : MonoBehaviour where T: MonoBehaviour
    {
        private static T _instance;

        public static T Ins
        {
            get {return _instance;}
        }

        protected virtual void Awake()
        {
            if(!_instance)
                _instance = this as T;
            else
                Destroy(this);
        }
	
    }
}
