using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UIManage.Animaiton
{
    public class TitleAnim : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private float moveDistanceUp = 20f;
        [SerializeField] private float moveDistanceDown = 20f;
        [SerializeField] private float duration = 0.5f;
        [SerializeField] private float pauseBetween = 0.1f;

        public GameObject titleUI;
        public GameObject lineUI;
        public Button startButton;
        private Vector3 _titleOriginalPosition;
        private Vector3 _lineOriginalPosition;
        private Feeding _feeding;
        private void Awake()
        {
            _titleOriginalPosition = titleUI.transform.localPosition;
            _lineOriginalPosition = lineUI.transform.localPosition;
        }

        private void Start()
        {
            Feeding.isEnable = false;
            // 绑定一个合并后的动画方法
            startButton.onClick.AddListener(PlayCombinedAnimation);
            PlayTitleScaleAndRotate(titleUI.transform,5);
        }

        private Sequence PlayTitleScaleAndRotate(Transform target, float duration, int loops = -1, LoopType loopType = LoopType.Yoyo)
        {
            Sequence scaleRotateSequence = DOTween.Sequence();
    
            // ===== 1. 缩放动画（0.65 → 0.6） =====
            Sequence scaleSequence = DOTween.Sequence();
            scaleSequence.Append(
                target.DOScale(new Vector3(0.61f, 0.61f, 0.61f), duration)
                    .SetEase(Ease.OutQuad)
            );
            scaleSequence.Append(
                target.DOScale(new Vector3(0.6f, 0.6f, 0.6f), duration)
                    .SetEase(Ease.InQuad)
            );

            // ===== 2. 旋转动画（流畅循环关键） =====
            Sequence rotateSequence = DOTween.Sequence();
            // 第一阶段：从0°旋转到1°（平滑开始）
            rotateSequence.Append(
                target.DORotate(new Vector3(0, 0, 1f), duration * 0.5f)
                    .SetEase(Ease.OutSine)
            );
            // 第二阶段：从1°旋转到-1°（平滑转向）
            rotateSequence.Append(
                target.DORotate(new Vector3(0, 0, -1f), duration)
                    .SetEase(Ease.InOutSine)
            );
            // 第三阶段：从-1°回到0°（平滑结束）
            rotateSequence.Append(
                target.DORotate(Vector3.zero, duration * 0.5f)
                    .SetEase(Ease.InSine)
            );

            // ===== 合并动画 =====
            scaleRotateSequence.Join(scaleSequence);
            scaleRotateSequence.Join(rotateSequence);
            scaleRotateSequence.SetLoops(loops, loopType);
    
            return scaleRotateSequence;
        }
        public void PlayCombinedAnimation()
        {
            // 停止所有动画
            titleUI.transform.DOKill();
            lineUI.transform.DOKill();

            // 重置位置
            titleUI.transform.localPosition = _titleOriginalPosition;
            lineUI.transform.localPosition = _lineOriginalPosition;

            // 创建主序列
            Sequence combinedSequence = DOTween.Sequence();

            // 1. 标题动画（上升→暂停→下降）
            Sequence titleSequence = DOTween.Sequence();
            titleSequence.Append(titleUI.transform.DOLocalMoveY(_titleOriginalPosition.y + moveDistanceUp, duration).SetEase(Ease.OutQuad));
            titleSequence.AppendInterval(pauseBetween);
            titleSequence.Append(titleUI.transform.DOLocalMoveY(_titleOriginalPosition.y - moveDistanceDown, duration).SetEase(Ease.InQuad));

            // 2. 线条动画（上升）
            Sequence lineSequence = DOTween.Sequence();
            lineSequence.Append(lineUI.transform.DOLocalMoveY(_lineOriginalPosition.y + moveDistanceDown, duration + 1).SetEase(Ease.OutBack));

            // 将两个子序列按顺序加入主序列
            combinedSequence.Append(titleSequence); // 先播放标题动画
            combinedSequence.Append(lineSequence);  // 标题动画完成后播放线条动画

            // 可选：添加回调
            combinedSequence.OnComplete(() => Debug.Log("All animations finished!"));
            Feeding.isEnable = true;
            StartController._isStart = false;
        }

        private void OnDisable()
        {
            titleUI.transform.DOKill();
            lineUI.transform.DOKill();
        }
    }
}