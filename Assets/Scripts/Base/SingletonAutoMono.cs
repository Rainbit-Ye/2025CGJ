using UnityEngine;

namespace Base
{
    /// <summary>
    /// 实现单例 Mono
    /// 会自动挂载在场景里并保证场景切换时不会被销毁
    /// 获取时才会自动挂在场景里
    /// </summary>
    /// <typeparam name="T">需继承MonoBehaviour</typeparam>
    public class SingletonAutoMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
    
        public static T Ins
        {
            get
            {
                if( _instance == null )
                {
                    GameObject obj = new GameObject();
                    // 重命名
                    obj.name = typeof(T).ToString();
                    _instance = obj.AddComponent<T>();
                }
                return _instance;
            }
        
        }

    }
}
