using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
            
            // 씬 전환 이벤트 등록
            SceneManager.sceneLoaded += OnSceneLoaded;
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



    /// <summary>
    /// Destory가 호출되는 경우 씬 전환 이벤트 해제
    /// </summary>
    private void OnDestroy()
    {
        // 이벤트 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }



    /// <summary>
    /// 씬 전환 이벤트 연결
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FadeIn();
    }




    /// <summary>
    /// 페이드 아웃
    /// </summary>
    /// <param name="onComplete">페이드 아웃 완료 후 실행할 함수</param>
    public void FadeOut(Action onComplete = null)
    {
        StartCoroutine(FadeOutCoroutine(onComplete));
    }




    /// <summary>
    /// 페이드 인
    /// </summary>
    /// <param name="onComplete">페이드 인 완료 후 실행할 함수</param>
    public void FadeIn(Action onComplete = null)
    {
        StartCoroutine(FadeInCoroutine(onComplete));
    }



    /// <summary>
    /// 페이드 인 코루틴
    /// </summary>
    /// <param name="onComplete">페이드 인 완료 후 실행할 함수</param>
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



    /// <summary>
    /// 페이드 아웃 코루틴
    /// </summary>
    /// <param name="onComplete">페이드 아웃 완료 후 실행할 함수</param>
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



    /// <summary>
    /// 이미지의 투명도 설정
    /// </summary>
    /// <param name="image">설정할 이미지</param>
    /// <param name="alpha">설정할 투명도</param>
    private void SetAlpha(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}
