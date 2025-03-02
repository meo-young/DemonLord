using UnityEngine;

public class AnimationNotify : MonoBehaviour
{
    // 전투 활성화 함수
    public void ActiveBattle()
    {
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
        FadeManager.instance.FadeOut(
            () =>
            {
                BattleManager.instance.StartBattle();
            }
        );
    }
}
