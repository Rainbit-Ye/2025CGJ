using UnityEngine;

namespace Global 
{
    [RequireComponent(typeof(Canvas))]
    public class AdaptiveCanvas : MonoBehaviour
    {
        public float factor;

        private Canvas _canvas;
        private float _width;
        private float _height;

        private float DEFULT_WIDTH = 1920;
        private float DEFULT_HEIGHT = 1080;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            InitCanvas();
            _canvas.scaleFactor = AdaptFactor();
        }


        private void Update()
        {
            if(InitCanvas())
                _canvas.scaleFactor = AdaptFactor();
        }
        
        /// <summary>
        /// 检查屏幕尺寸变化并获取
        /// </summary>
        /// <returns>有变化为true</returns>
        private bool InitCanvas()
        {
            if (!Mathf.Approximately(_width, Screen.width) || !Mathf.Approximately(_height, Screen.height))
            {
                _width = Screen.width;
                _height = Screen.height;
                return true;
            }
            return false;
        }

        private float AdaptFactor()
        {
            float factorHei = _height / DEFULT_HEIGHT;
            float factorWid = _width / DEFULT_WIDTH;
            factor = Mathf.Min(factorHei, factorWid);
            //Debug.Log(factor);
            return factor;
        }

    }
}