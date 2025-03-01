using System;
using UnityEngine;

public class LDHBaseUIData
{
    public Action OnShow;
    public Action OnClose;
}

public class LDHBaseUI : MonoBehaviour
{
    public Animation m_UIOpenAnim;

    private Action m_OnShow;
    private Action m_OnClose;

    public virtual void Init(Transform anchor)
    {
        m_OnShow = null;
        m_OnClose = null;

        transform.SetParent(anchor);

        var rectTransform = GetComponent<RectTransform>();
        rectTransform.localPosition = Vector3.zero;
        rectTransform.localScale = Vector3.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }
    public virtual void SetInfo(LDHBaseUIData uiData)
    {
        m_OnShow = uiData.OnShow;
        m_OnClose = uiData.OnClose;
    }
    public virtual void ShowUI()
    {
        if (m_UIOpenAnim)
        {
            m_UIOpenAnim.Play();
        }
        m_OnShow?.Invoke();
        m_OnShow = null;
    }
    public virtual void CloseUI(bool isCloseAll = false)
    {
        if (!isCloseAll)
        {
            m_OnClose?.Invoke();
        }
        m_OnClose = null;

        LDHUIManager.instance.CloseUI(this);
    }
    public virtual void OnClickCloseButton()
    {
        //AudioManager.Instance.PlaySFX(SFX.sfx_click);
        CloseUI();
    }
}
