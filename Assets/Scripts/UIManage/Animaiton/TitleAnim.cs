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

        private void Awake()
        {
            _titleOriginalPosition = titleUI.transform.localPosition;
            _lineOriginalPosition = lineUI.transform.localPosition;
        }

        private void Start()
        {
            // 绑定一个合并后的动画方法
            startButton.onClick.AddListener(PlayCombinedAnimation);
        }

        // 合并后的动画：先播放标题动画，再播放线条动画
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
        }

        private void OnDisable()
        {
            titleUI.transform.DOKill();
            lineUI.transform.DOKill();
        }
    }
}