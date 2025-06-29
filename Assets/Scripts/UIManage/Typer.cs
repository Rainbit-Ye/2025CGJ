using UnityEngine;
using TMPro;
using System.Collections;
using Music;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpText; // TextMeshPro组件
    [SerializeField] private float charDelay = 0.05f;
    [SerializeField] private float punctuationDelay = 0.3f;
    [SerializeField] private AudioClip typeSound; // 可选：打字音效

    private string fullText;
    private bool isTyping = false;

    void Start()
    {
        fullText = tmpText.text;
        tmpText.text = "";
        StartCoroutine(TypeText());
    }

    void Update()
    {
        // 点击鼠标或屏幕时加速显示
        if (Input.GetMouseButtonDown(0) && isTyping)
        {
            StopAllCoroutines();
            tmpText.text = fullText;
            isTyping = false;
        }
    }

    IEnumerator TypeText()
    {
        isTyping = true;
        for (int i = 0; i < fullText.Length; i++)
        {
            tmpText.text += fullText[i];

            // 播放音效（可选）
            if (typeSound != null && !char.IsWhiteSpace(fullText[i]))
            {
                
            }

            // 动态延迟
            float delay = IsPunctuation(fullText[i]) ? punctuationDelay : charDelay;
            yield return new WaitForSeconds(delay);
        }
        isTyping = false;
    }

    private bool IsPunctuation(char c)
    {
        return c == ',' || c == '.' || c == '!' || c == '?' || c == ';' || c == ':';
    }
}