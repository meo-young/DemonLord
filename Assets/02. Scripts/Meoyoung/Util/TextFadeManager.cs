using UnityEngine;
using TMPro;
using System.Collections;

public class TextFadeManager : MonoBehaviour
{
    public static TextFadeManager instance;

    [Header("텍스트 페이드 시간")]
    [SerializeField] private float fadeDuration = 2f;

    private void Awake()
    {
        instance = this;
    }

    public void FadeIn(TMP_Text text)
    {
        StartCoroutine(FadeInCoroutine(text));
    }

    public void FadeOut(TMP_Text text)
    {
        StartCoroutine(FadeOutCoroutine(text));
    }

    private IEnumerator FadeInCoroutine(TMP_Text text)
    {
        float elapsedTime = 0f;
        Color currentColor = text.color;
        currentColor.a = 0f;
        text.color = currentColor;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            currentColor.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            text.color = currentColor;
            yield return null;
        }

        currentColor.a = 1f;
        text.color = currentColor;
    }

    private IEnumerator FadeOutCoroutine(TMP_Text text)
    {
        float elapsedTime = 0f;
        Color currentColor = text.color;
        currentColor.a = 1f;
        text.color = currentColor;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            currentColor.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            text.color = currentColor;
            yield return null;
        }

        currentColor.a = 0f;
        text.color = currentColor;
    }
}
