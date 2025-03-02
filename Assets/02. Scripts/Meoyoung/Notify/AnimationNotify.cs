using UnityEngine;

public class AnimationNotify : MonoBehaviour
{

    // 전투 시작 함수
    public void StartBattle()
    {
        PartyRecruitUI.instance.HideRecruitPanel();
        BattleManager.instance.StartBattle();
    }
    
    // 전투 활성화 함수
    public void ActiveBattle()
    {
        GameManager.instance.SetInactiveWarrant();
        BattleManager.instance.ActiveBattle();
    }

    // 전투 텍스트 애니메이션 시작 함수
    public void StartBattleTextAnim()
    {
        AnimManager.instance.StartBattleTextAnim();
    }

    // 전투 텍스트 애니메이션 종료 함수
    public void EndBattleTextAnim()
    {
        AnimManager.instance.EndBattleTextAnim();
    }

    // 페이드 인 함수
    public void BattleFadeIn()
    {
        FadeManager.instance.FadeIn();
    }

    // 전투가 끝난 후 파티 이탈이 종료되면 새로운 배틀 시작
    public void BattleFadeOut()
    {
        BattleManager.instance.PassOverBattle();
    }

    // 전투 승리시 SFX 출력
    public void PlayWinSFX()
    {
        AudioManager.instance.StopBGM();
        AudioManager.instance.PlaySFX(SFX.sfx_clear);
    }
}
