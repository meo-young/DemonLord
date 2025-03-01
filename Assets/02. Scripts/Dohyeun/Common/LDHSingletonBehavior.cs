using UnityEngine;

// 미리 만들어두지 않으면 제대로 못 쓰는 하자 좀 있음

public abstract class LDHSingletonBehavior<T> : MonoBehaviour where T : LDHSingletonBehavior<T>
{
    protected bool _isDestroyOnLoad = false;
    protected bool isInitialized = false;

    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = GameObject.Find(typeof(T).ToString());
                if (go != null)
                {
                    if (go.TryGetComponent<T>(out T targetComponent))
                    {
                        instance = targetComponent;
                        instance.Init(); // T의 Init() 호출
                    }
                    else
                    {
                        // T 컴포넌트를 붙이고, T의 Init() 호출
                        instance = go.AddComponent<T>();
                        instance.Init();
                    }
                }
                else
                {
                    // 새 오브젝트 생성
                    go = new GameObject(typeof(T).ToString());
                    instance = go.AddComponent<T>();

                    // Init() 호출
                    instance.Init();

                    // isDestroyOnLoad에 따라 Parent 설정
                    GameObject parent = GetOrCreateParent(instance._isDestroyOnLoad);
                    go.transform.SetParent(parent.transform);
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        Init();
    }
    protected virtual void Init()
    {
        if (instance == null)
        {
            instance = (T)this;
            if (!_isDestroyOnLoad)
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
            instance.InitChild();
        isInitialized = true;
    }

    protected abstract void InitChild();

    protected virtual void OnDestroy()
    {
        Dispose();
    }
    protected virtual void Dispose()
    {
        instance = null;
    }
    private static GameObject GetOrCreateParent(bool isDestroyOnLoad)
    {
        // Parent 이름 설정
        string parentName = isDestroyOnLoad ? "[Scripts-Destroy]" : "[Scripts-DontDestroy]";

        // Parent 오브젝트 찾기 또는 생성
        GameObject parent = GameObject.Find(parentName);
        if (parent == null)
        {
            parent = new GameObject(parentName);
        }

        return parent;
    }
}
