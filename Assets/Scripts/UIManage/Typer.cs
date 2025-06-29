using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpText;
    [SerializeField] private float charDelay = 0.05f;
    [SerializeField] private float punctuationDelay = 0.3f;
    [SerializeField] private AudioClip typeSound;
    [SerializeField] private string[] sentences;
    [SerializeField] private float delayBetweenSentences = 3f;

    private int currentSentenceIndex = 0;
    private bool isTyping = false;
    private AudioSource audioSource;
    private Coroutine typingCoroutine;

    void OnEnable()
    {
        Initialize();
        StartTyping();
    }

    void OnDisable()
    {
        StopTyping();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isTyping)
        {
            SkipTyping();
        }
    }

    private void Initialize()
    {
        currentSentenceIndex = 0;
        
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.ignoreListenerPause = true;
            audioSource.playOnAwake = false;
        }
        
        tmpText.text = "";
    }

    private void StartTyping()
    {
        StopTyping();
        typingCoroutine = StartCoroutine(TypeAllSentences());
    }

    private void StopTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        isTyping = false;
    }

    private void SkipTyping()
    {
        StopTyping();
        tmpText.text = sentences[currentSentenceIndex];
        StartCoroutine(ShowNextSentenceAfterDelay(0));
    }

    IEnumerator TypeAllSentences()
    {
        while (currentSentenceIndex < sentences.Length)
        {
            yield return TypeSingleSentence(sentences[currentSentenceIndex]);
            currentSentenceIndex++;
            
            if (currentSentenceIndex < sentences.Length)
            {
                yield return new WaitForSecondsRealtime(delayBetweenSentences);
                tmpText.text = "";
            }
        }
    }

    IEnumerator TypeSingleSentence(string sentence)
    {
        isTyping = true;
        tmpText.text = "";
        
        for (int i = 0; i < sentence.Length; i++)
        {
            tmpText.text += sentence[i];

            if (typeSound != null && !char.IsWhiteSpace(sentence[i]))
            {
                audioSource.PlayOneShot(typeSound);
            }

            float delay = IsPunctuation(sentence[i]) ? punctuationDelay : charDelay;
            yield return new WaitForSecondsRealtime(delay);
        }
        
        isTyping = false;
    }

    IEnumerator ShowNextSentenceAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        currentSentenceIndex++;
        if (currentSentenceIndex < sentences.Length)
        {
            tmpText.text = "";
            typingCoroutine = StartCoroutine(TypeSingleSentence(sentences[currentSentenceIndex]));
        }
    }

    private bool IsPunctuation(char c)
    {
        return c == ',' || c == '.' || c == '!' || c == '?' || c == ';' || c == ':';
    }
}