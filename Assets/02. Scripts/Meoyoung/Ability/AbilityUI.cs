using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityUI : MonoBehaviour
{
    [Header("특성 결정 버튼")]
    [SerializeField] private Button confirmAbilityButton;

    [Header("현재 특성 설명 텍스트")]
    [SerializeField] private TMP_Text currentAbilityText;

    public Image[] abilityCard;

    private int currentAbilityCount = -1;



    /// <summary>
    /// 특성 카드를 클릭할 시 결정하기 버튼 활성화
    /// </summary>
    /// <param name="count"> 클릭한 특성 카드 인덱스</param>
    public void SetAbilityCount(int count)
    {
        currentAbilityCount = count;
        confirmAbilityButton.interactable = true;

        InitAbilityUI();
    }



    /// <summary>
    /// 특성 결정 버튼 클릭 시 특성 설정 및 UI 비활성화
    /// </summary>
    public void SetAbility()
    {
        // 게임의 메인 특성 설정
        GameManager.instance.SetCurrentAbility((AbilityType)currentAbilityCount);

        // 특성 UI 비활성화
        DeactiveAbilityUI();

        // 해당 특성의 설명 가져오기
        currentAbilityText.text = AbilityManager.instance.GetAbilityDescription((AbilityType)currentAbilityCount);
    }    



    /// <summary>
    /// 특성 UI 비활성화
    /// </summary>
    private void DeactiveAbilityUI()
    {
        AbilityWarrantUI.instance.ProcessEventQueue();
    }



    /// <summary>
    /// 특성 UI 초기화
    /// </summary>
    private void InitAbilityUI()
    {
        if(currentAbilityCount < 0 || currentAbilityCount >= abilityCard.Length)
        {
            return;
        }

        foreach(Image card in abilityCard)
        {
            card.sprite = GameManager.instance.cardBack;
        }

        abilityCard[currentAbilityCount].sprite = GameManager.instance.cardFront;
    }
}
