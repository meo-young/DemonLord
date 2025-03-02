using UnityEngine;
using System;
using UnityEngine.UI;

public class NPCSelectionUI : MonoBehaviour
{
    [Header("결정 버튼")]
    [SerializeField] private Button decisionButton;


    public void OnClickCheckBtn()
    {
        // 결정 버튼 활성화
        decisionButton.interactable = true;

        Dice.instance.AddDiceResultEvent(CheckNPCEvent);
    }




    /// <summary>
    /// 패스 버튼 클릭 시 패스 이벤트 등록
    /// </summary>
    public void OnClickPassByBtn()
    {
        // 결정 버튼 활성화
        decisionButton.interactable = true;

        Dice.instance.AddDiceResultEvent(PassByEvent);
    }


    
    /// <summary>
    /// FadeOut 연출 후 다음 스테이지로 넘어감
    /// </summary>
    private void PassByEvent()
    {
        TempFadeOut();
    }



    /// <summary>
    /// UI 숨김
    /// </summary>
    private void HideUI()
    {
        transform.localScale = Vector3.zero;
    }



    /// <summary>
    /// 주사위 굴린 후 어떤 이벤트인지 텍스트 출력
    /// </summary>
    private void CheckNPCEvent()
    {
        int diceValue = GameManager.instance.currentDiceResultInt;

        if (diceValue == 2)
        {
            SituationUI.instance.SetSituationText("마왕에게 바로 가자");
        }
        else if (diceValue >= 3 && diceValue <= 4) 
        {
            SituationUI.instance.SetSituationText("마왕의 부하래래");
        }
        else if (diceValue == 5)
        {
            SituationUI.instance.SetSituationText("지나가던 행인이래");
        }
        else if (diceValue >= 6 && diceValue <= 7)
        {
            SituationUI.instance.SetSituationText("하위 프리스트래");
        }
        else if (diceValue >= 9 && diceValue <= 11)
        {
            SituationUI.instance.SetSituationText("고위 프리스트래");
        }
        else if (diceValue == 12)
        {
            SituationUI.instance.SetSituationText("위대한 가호래");
        }

        Invoke("TempFadeOut", 1.5f);
    }

    private void TempFadeOut()
    {
        FadeManager.instance.FadeOut(
            () =>
            {
                HideUI();
                BattleManager.instance.StartBattle();
            }
        );
    }

    
}
