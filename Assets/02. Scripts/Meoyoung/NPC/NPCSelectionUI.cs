using UnityEngine;
using System;
using UnityEngine.UI;
using static Constant;

public class NPCSelectionUI : MonoBehaviour
{
    public static NPCSelectionUI instance;

    private void Awake()
    {
        instance = this;
    }


    public void OnClickCheckBtn()
    {
        Dice.instance.AddDiceResultEvent(CheckNPCEvent);
    }




    /// <summary>
    /// 패스 버튼 클릭 시 패스 이벤트 등록
    /// </summary>
    public void OnClickPassByBtn()
    {
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
    /// NPC 이벤트 활성화
    /// </summary>
    public void ShowNPCSelectionUI()
    {
        // 주사위 결과 텍스트 초기화
        Dice.instance.InitDiceBtn();

        // UI 활성화
        transform.localScale = Vector3.one;

        // 상황 텍스트 설정
        SituationUI.instance.SetSituationText("의문의 NPC와 조우했습니다.");
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

        if (diceValue <= 4) 
        {
            AudioManager.instance.PlaySFX(SFX.sfx_NPC_Fail);
            SituationUI.instance.SetSituationText("마왕의 부하였습니다. 전체가 -5hp의 피해를 입습니다.");
            PartyManager.instance.GetDamageAlivePartyMembers(NPC_DAMAGE_AMOUNT);
        }
        else if (diceValue == 5)
        {
            SituationUI.instance.SetSituationText("지나가던 행인이었습니다. 아무런 효과가 없습니다.");
        }
        else if (diceValue >= 6 && diceValue <= 11)
        {
            SituationUI.instance.SetSituationText("고위 프리스트에게 치료를 받습니다. 파티원 전체가 +20hp가 회복됩니다.");
            PartyManager.instance.HealAlivePartyMembers(NPC_HEAL_AMOUNT);
        }
        else if (diceValue == 12)
        {
            SituationUI.instance.SetSituationText("위대한 가호를 받습니다. 파티원 전체가 부활하고 파티원 전체의 체력이 100%가 됩니다.");
            PartyManager.instance.RebirthPartyMembers();
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
