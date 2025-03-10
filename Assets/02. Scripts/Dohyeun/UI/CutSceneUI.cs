using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutSceneUIData : LDHBaseUIData
{
    public string Id;
}
public class CutSceneUI : LDHBaseUI
{
    private CutSceneUIData m_CutsceneUIData;
    private Queue<OneCutSceneData> m_CutsceneDatas;
    private Queue<string> m_Texts = new Queue<string>();
    private OneCutSceneData m_CurrentCutsceneData;

    [SerializeField] private Image m_Background;
    [SerializeField] private TextMeshProUGUI m_Text;

    public override void SetInfo(LDHBaseUIData uiData)
    {
        base.SetInfo(uiData);
        m_CutsceneUIData = uiData as CutSceneUIData;
        m_CutsceneDatas = new Queue<OneCutSceneData>(LDHUIManager.instance.GetCutsceneData(m_CutsceneUIData.Id).OneCutscenes);
        SetNext();
    }
    public bool SetNext()
    {
        if (m_Texts.Count == 0)
        {
            if (m_CutsceneDatas.Count == 0)
            {
                CloseUI();
                return false;
            }
            var nextData = m_CutsceneDatas.Dequeue();
            foreach (var data in nextData.Texts)
                m_Texts.Enqueue(data);
            m_Background.sprite = nextData.Background;
            CompleteText = "";
            m_Text.rectTransform.anchoredPosition = nextData.TextPosition;
        }
        m_Text.text = CompleteText;
        string nextLine = m_Texts.Dequeue();
        if (CompleteText != "") CompleteText += "\n";
        CompleteText += nextLine;
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText($"{nextLine}"));
        return true;
    }
    public void OnClickScreen()
    {
        SetNext();
    }
    [Header("출력 속도")]
    [SerializeField] private float typingSpeed = 0.05f;
    private Coroutine typingCoroutine;
    private string CompleteText;

    private IEnumerator TypeText(string fullText)
    {
        if (m_Text.text != "") m_Text.text += "\n";

        // 꺽쇠 괄호 플래그
        bool LFlag = false;
        foreach (char letter in fullText)
        {
            m_Text.text += letter;
            if (letter == '<') LFlag = true;
            if (letter == '>') LFlag = false;
            if (letter == '\\') continue;
            if (LFlag) continue;
                yield return new WaitForSeconds(typingSpeed);
        }
        typingCoroutine = null;
    }
}
