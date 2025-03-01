using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LDHUIManager : MonoBehaviour
{

    public static LDHUIManager instance;

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
    }

    public Transform UICanvasTrs;
    public Transform ClosedUITrs;

    private LDHBaseUI m_FrontUI; // 최상단 UI

    private Dictionary<System.Type, GameObject> m_OpenUIPool = new Dictionary<System.Type, GameObject>();
    private Dictionary<System.Type, GameObject> m_ClosedUIPool = new Dictionary<System.Type, GameObject>();

    [SerializeField] private CutSceneSO cutSceneSO;

    private LDHBaseUI GetUI<T>(out bool isAlreadyOpen)
    {
        System.Type uiType = typeof(T);
        LDHBaseUI ui = null;
        isAlreadyOpen = false;
        if (m_OpenUIPool.ContainsKey(uiType))
        {
            ui = m_OpenUIPool[uiType].GetComponent<LDHBaseUI>();
            isAlreadyOpen = true;
        }
        else if (m_ClosedUIPool.ContainsKey(uiType))
        {
            ui = m_ClosedUIPool[uiType].GetComponent<LDHBaseUI>();
            m_ClosedUIPool.Remove(uiType);
        }
        else
        {
            var uiObj = Instantiate(Resources.Load($"{Path.Combine(SubUtils.BASE_PATH, SubUtils.UI_PATH)}/{uiType}", typeof(GameObject))) as GameObject;
            ui = uiObj.GetComponent<LDHBaseUI>();
        }
        return ui;
    }
    public void OpenUI<T>(LDHBaseUIData uiData)
    {
        System.Type uiType = typeof(T);

        bool isAlreadyOpen = false;
        var ui = GetUI<T>(out isAlreadyOpen);

        if (!ui)
        {
            Debug.LogError($"{uiType} does not exist.");
            return;
        }
        if (isAlreadyOpen)
        {
            Debug.LogError($"{uiType} is already open.");
            return;
        }
        var siblingIndex = UICanvasTrs.childCount;
        ui.Init(UICanvasTrs);
        ui.transform.SetSiblingIndex(siblingIndex);
        ui.gameObject.SetActive(true);
        ui.SetInfo(uiData);
        ui.ShowUI();

        m_FrontUI = ui;
        m_OpenUIPool[uiType] = ui.gameObject;
    }
    public void CloseUI(LDHBaseUI ui)
    {
        System.Type uiType = ui.GetType();
        ui.gameObject.SetActive(false);
        m_OpenUIPool.Remove(uiType);
        m_ClosedUIPool[uiType] = ui.gameObject;
        ui.transform.SetParent(ClosedUITrs);
        m_FrontUI = null;
        var lastChild = UICanvasTrs.GetChild(UICanvasTrs.childCount - 1);
        if (lastChild)
        {
            m_FrontUI = lastChild.gameObject.GetComponent<LDHBaseUI>();
        }
    }
    public LDHBaseUI GetActiveUI<T>()
    {
        var uiType = typeof(T);
        return m_OpenUIPool.ContainsKey(uiType) ? m_OpenUIPool[uiType].GetComponent<LDHBaseUI>() : null;
    }
    public bool ExistsOpenUI()
    {
        return m_FrontUI != null;
    }
    public LDHBaseUI GetCurrentFrontUI()
    {
        return m_FrontUI;
    }
    public void CloseCurrentFrontUI()
    {
        m_FrontUI.CloseUI();
    }
    public void CloseAllOpenUI()
    {
        while (m_FrontUI)
        {
            m_FrontUI.CloseUI(true);
        }
    }
    public void ShowCutScene(string id)
    {
        CutSceneUIData uiData = new CutSceneUIData() { Id = id };
        OpenUI<CutSceneUI>(uiData);
    }
    // 데이터 관리 매니저가 없어 해당 위치 작성
    public CutSceneData GetCutsceneData(string id) => cutSceneSO.CutsceneDatas.Where(item => item.Id == id).FirstOrDefault();
}
