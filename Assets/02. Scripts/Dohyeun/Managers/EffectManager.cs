using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    #region 싱글톤 공통
    private bool isDestroyOnLoad = false;
    private bool isInitialized = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (!isDestroyOnLoad)
            {
                if (transform.parent)
                    DontDestroyOnLoad(transform.parent);
                else
                    DontDestroyOnLoad(this);
            }
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        if (!isInitialized)
            instance.Initialize();
        isInitialized = true;
    }
    #endregion
    private void Initialize()
    {
        LoadAllEffects();
        OnScreenClick = null;
        OnScreenClick += ShowEffect;
    }
    // Resources 내 데이터 경로
    private string resourcesPath = Path.Combine(SubUtils.BASE_PATH, SubUtils.EFFECT_PATH);

    // 전체 프리팹
    private Dictionary<string, GameObject> EffectDict = new Dictionary<string, GameObject>();

    // Resoruces Path에서 프리팹 모두 로드
    private void LoadAllEffects()
    {
        var loaded = Resources.LoadAll<GameObject>(resourcesPath);
        foreach(var go in loaded)
        {
            Debug.Log(go.name);
            EffectDict.Add(go.name, go);
        }
    }
    // 파티클 이펙트 Instantiate, 효과 종료 시 Destory
    // TODO:풀링 및 Play로 디벨롭
    public void ShowEffect(string key, Vector3 pos) => ShowEffect(key, (Vector2)pos);
    public void ShowEffect(string key, Vector2 pos)
    {
        Instantiate(EffectDict[key], pos, Quaternion.identity);
        // 종료 시, Destory는 EffectObject 컴포넌트가 처리하도록 되었음. 풀링으로 디벨롭 시 확인하기
    }

    // 특정 상황에 대응할 액션
    public delegate void ClickAction(string key, Vector3 worldPos);
    public event ClickAction OnScreenClick;


    // MouceClick 이펙트


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = GetWorldPosition(Input.mousePosition);
            OnScreenClick?.Invoke("TestMouseClickEffect01", worldPos);
        }
    }
    private Vector3 GetWorldPosition(Vector3 screenPosition)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPos.z = 0f;
        return worldPos;
    }

}
