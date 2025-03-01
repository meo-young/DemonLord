using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public static FadeManager instance;

    [Header("페이드 전환 시간")]
    [SerializeField] private float fadeDuration;

    private Image defaultImage; // Fade를 적용할 기본 이미지

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // 캔버스의 카메라를 메인 카메라로 설정
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;

        // 페이드 이미지 초기화
        defaultImage = GetComponentInChildren<Image>();
        defaultImage.transform.localScale = Vector3.zero;
    }
    
    public void FadeOut(Action onComplete = null)
    {
        StartCoroutine(FadeOutCoroutine(onComplete));
    }

    public void FadeIn(Action onComplete = null)
    {
        StartCoroutine(FadeInCoroutine(onComplete));
    }

    private IEnumerator FadeInCoroutine(Action onComplete)
    {
        defaultImage.transform.localScale = Vector3.one;

        // 이미지 투명도 1로 초기화
        float elapsedTime = 0f;
        SetAlpha(defaultImage, 1f);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            SetAlpha(defaultImage, alpha);
            yield return null;
        }

        // 오차방지 차원에서 투명도를 0으로 설정
        SetAlpha(defaultImage, 0f);

        // 이미지 비활성화
        defaultImage.transform.localScale = Vector3.zero;

        // fadeDuration만큼의 딜레이
        yield return new WaitForSeconds(fadeDuration);

        // 콜백함수 호출
        onComplete?.Invoke();
    }

    private IEnumerator FadeOutCoroutine(Action onComplete)
    {
        defaultImage.transform.localScale = Vector3.one;

        // 이미지 투명도 0으로 초기화화
        float elapsedTime = 0f;
        SetAlpha(defaultImage, 0f);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);

            SetAlpha(defaultImage, alpha);
            yield return null;
        }

        // 오차방지 차원에서 투명도를 1로 설정
        SetAlpha(defaultImage, 1f);

        // fadeDuration만큼의 딜레이
        yield return new WaitForSeconds(fadeDuration);

        // 콜백함수 호출
        onComplete?.Invoke();
    }

    private void SetAlpha(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    
}
