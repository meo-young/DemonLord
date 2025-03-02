using UnityEngine;
using TMPro;

public class StageInfoUI : MonoBehaviour
{
    public static StageInfoUI instance;

    private TMP_Text stageNameText;

    private void Awake()
    {
        instance = this;

        // 스테이지 이름 텍스트 컴포넌트 찾기
        stageNameText = GetComponentInChildren<TMP_Text>();
    }

    public void SetStageInfo(string name)
    {
        // 스테이지 정보 설정
        stageNameText.text = name;
    }
    
}
