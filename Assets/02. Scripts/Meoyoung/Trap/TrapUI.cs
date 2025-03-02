using UnityEngine;
using static Constant;

public class TrapUI : MonoBehaviour
{
    public static TrapUI instance;

    private void Awake()
    {
        instance = this;
    }



    /// <summary>
    /// 함정 UI 활성화
    /// </summary>
    public void ShowTrapUI()
    {
        // 주사위 결과 텍스트 초기화
        Dice.instance.InitDiceBtn();

        // UI 활성화
        transform.localScale = Vector3.one;
        
        // 상황 텍스트 수정
        SituationUI.instance.SetSituationText("파티가 함정에 빠지게 되었습니다.");
    }



    /// <summary>
    /// 도적의 가호 버튼 클릭 시 이벤트 추가
    /// </summary>
    public void OnClickTrapUI()
    {
        // 주사위 이벤트 추가
        Dice.instance.AddDiceResultEvent(CheckTrapEvent);
    }




    /// <summary>
    /// 함정 UI 숨김
    /// </summary>
    private void HideTrapUI()
    {
        transform.localScale = Vector3.zero;
    }

    

    /// <summary>
    /// 주사위 굴린 후 어떤 이벤트인지 텍스트 출력
    /// </summary>
    private void CheckTrapEvent()
    {
        int diceValue = GameManager.instance.currentDiceResultInt;

        int trapHandicap = GameManager.instance.trapHandicap;

        if(GameManager.instance.isThiefItem)
        {
            if (diceValue <= 3 - trapHandicap)
            {
                AttackAllPartyMembers();
                AudioManager.instance.PlaySFX(SFX.sfx_Trapped);
                SituationUI.instance.SetSituationText("파티원들의 체력이 5 감소하였습니다 !");
            }
            else
            {
                SituationUI.instance.SetSituationText("무사히 탈출에 성공하였습니다");
                AudioManager.instance.PlaySFX(SFX.sfx_NoTrapped);
            }
        }
        else
        {
            if (diceValue <= 6 - trapHandicap)
            {
                AttackAllPartyMembers();
                AudioManager.instance.PlaySFX(SFX.sfx_Trapped);
                SituationUI.instance.SetSituationText("파티원들의 체력이 5 감소하였습니다 !");
            }
            else
            {
                SituationUI.instance.SetSituationText("무사히 탈출에 성공하였습니다");
                AudioManager.instance.PlaySFX(SFX.sfx_NoTrapped);
            }
        }
        

        Invoke("TempFadeOut", 1.5f);
    }

    private void AttackAllPartyMembers()
    {
        foreach (var member in PartyManager.instance.GetPartyMembers())
        {
            member.GetDamage(TRAP_DAMAGE_AMOUNT);
        }
    }



    private void TempFadeOut()
    {
        FadeManager.instance.FadeOut(
            () =>
            {
                HideTrapUI();
                BattleManager.instance.StartBattle();
            }
        );
    }
}
