using UnityEngine;

public class ThiefItem : MonoBehaviour
{
    public static ThiefItem instance;

    private void Awake()
    {
        instance = this;
    }



    /// <summary>
    /// 도적의 가호 UI 활성화
    /// </summary>
    public void ShowThiefItem()
    {
        // 주사위 결과 텍스트 초기화
        Dice.instance.InitDiceBtn();

        // UI 활성화
        transform.localScale = Vector3.one;
        
        // 상황 텍스트 수정
        SituationUI.instance.SetSituationText("도적의 가호는 데미지 증가와 함정에 유리해집니다.");
    }



    /// <summary>
    /// 도적의 가호 버튼 클릭 시 이벤트 추가
    /// </summary>
    public void OnClickThiefItem()
    {
        // 주사위 이벤트 추가
        Dice.instance.AddDiceResultEvent(CheckThiefItemEvent);
    }




    /// <summary>
    /// 도적의 가호 UI 숨김
    /// </summary>
    private void HideThiefItem()
    {
        transform.localScale = Vector3.zero;
    }

    

    /// <summary>
    /// 주사위 굴린 후 어떤 이벤트인지 텍스트 출력
    /// </summary>
    private void CheckThiefItemEvent()
    {
        int diceValue = GameManager.instance.currentDiceResultInt;

        if (diceValue <= 6)
        {
            SituationUI.instance.SetSituationText("도적의 가호를 받았습니다.");
            GameManager.instance.GetThiefItem();
        }
        else
        {
            SituationUI.instance.SetSituationText("도적의 가호를 받지 못했습니다...");
        }
        

        Invoke("TempFadeOut", 1.5f);
    }



    private void TempFadeOut()
    {
        FadeManager.instance.FadeOut(
            () =>
            {
                HideThiefItem();
                BattleManager.instance.StartBattle();
            }
        );
    }
}
