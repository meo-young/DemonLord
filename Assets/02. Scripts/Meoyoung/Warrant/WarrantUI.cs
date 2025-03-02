using UnityEngine;
using UnityEngine.UI;

public class WarrantUI : MonoBehaviour
{
    [Header("권능 결정 버튼")]
    [SerializeField] private Button confirmWarrantButton;

    private int currentWarrantCount = -1;



    /// <summary>
    /// 권능 카드를 클릭할 시 결정하기 버튼 활성화
    /// </summary>
    /// <param name="count"> 클릭한 권능 카드 인덱스</param>
    public void SetWarrantCount(int count)
    {
        currentWarrantCount = count;
        confirmWarrantButton.interactable = true;
    }



    /// <summary>
    /// 권능 결정 버튼 클릭 시 권능 설정 및 UI 비활성화
    /// </summary>
    public void SetWarrant()
    {
        GameManager.instance.SetCurrentWarrant((WarrantType)currentWarrantCount);
        DeactiveWarrantUI();
    }    



    /// <summary>
    /// 권능 UI 비활성화
    /// </summary>
    private void DeactiveWarrantUI()
    {
        AbilityWarrantUI.instance.ProcessEventQueue();
    }
}
