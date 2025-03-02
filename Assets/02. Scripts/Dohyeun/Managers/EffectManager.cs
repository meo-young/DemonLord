using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.VFX;

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
            //Debug.Log(go.name);
            EffectDict.Add(go.name, go);
        }
    }

    // 파티클 이펙트 Instantiate, 효과 종료 시 Destory
    // TODO:풀링 및 Play로 디벨롭
    public void ShowEffect(EffectType key, Vector3 pos) => ShowEffect(key.ToString(), (Vector2)pos);
    public void ShowEffect(EffectType key, Vector2 pos) => ShowEffect(key.ToString(), pos);
    public void ShowEffect(string key, Vector3 pos) => ShowEffect(key, (Vector2)pos);
    public void ShowEffect(string key, Vector2 pos)
    {
        if (!EffectDict.ContainsKey(key)) return;

        GameObject effectInstance = Instantiate(EffectDict[key], pos, Quaternion.identity);
        EffectObject effectObject = effectInstance.GetComponent<EffectObject>();
        effectObject.IsShowOneTime = true;
        effectObject.Initialize();

        if (effectObject == null) return;
    }

    // sec 시간 지속 후 Stop()
    public void ShowEffect(EffectType key, Vector3 pos, float sec) => ShowEffect(key.ToString(), (Vector2)pos, sec);
    public void ShowEffect(EffectType key, Vector2 pos, float sec) => ShowEffect(key.ToString(), pos, sec);
    public void ShowEffect(string key, Vector3 pos, float sec) => ShowEffect(key, (Vector2)pos, sec);
    public void ShowEffect(string key, Vector2 pos, float sec)
    {
        if (!EffectDict.ContainsKey(key)) return;

        GameObject effectInstance = Instantiate(EffectDict[key], pos, Quaternion.identity);
        EffectObject effectObject = effectInstance.GetComponent<EffectObject>();
        effectObject.IsShowOneTime = false;
        effectObject.Initialize();

        StartCoroutine(StopEffectAfterTime(effectObject, sec));
    }

    IEnumerator StopEffectAfterTime(EffectObject effectObject, float sec)
    {
        yield return new WaitForSeconds(sec);
        effectObject.StopEffect();
    }

    // 특정 상황에 대응할 액션
    public delegate void ClickAction(string key, Vector3 worldPos);
    public event ClickAction OnScreenClick;

    void Update()
    {
        // MouceClick 이펙트
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
public enum EffectType
{
    Damage_Melee,
    Damage_Range,
    Damage_Magic,
    Energe_Melee,
    Energe_Range,
    Energe_Magic,
    Mouse_Click,
}